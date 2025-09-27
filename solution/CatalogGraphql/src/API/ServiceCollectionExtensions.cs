using Application.AuthorCommands.CreateAuthor;
using Application.AuthorQueries;
using Application.AuthorQueries.GetAuthors;
using Application.BookQueries;
using Common.Domain;
using Domain;
using HotChocolate.Execution;
using Infra.Data;
using Infra.DataLoaders;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.ApplicationInsights.Extensibility;

namespace API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureLogging(configuration);
        services.ConfigureGraphql();
        services.ConfigureInfraServices(configuration);

        // Configure GraphQL with a single Query type containing all operations
        services.AddMediatR(conf => { conf.RegisterServicesFromAssemblyContaining<CreateAuthorHandler>(); });

        return services;
    }

    private static void ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Application Insights telemetry
        services.AddApplicationInsightsTelemetry(configuration);
        
        // Configure telemetry initializer
        services.AddSingleton<ITelemetryInitializer, GraphQLTelemetryInitializer>();
    }

    private static void ConfigureGraphql(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            .AddGraphQLServer()
            // Types
            .AddTypes()
            // Conventions
            .AddQueryConventions()
            .AddMutationConventions(new MutationConventionOptions
            {
                ApplyToAllMutations = true,
                InputArgumentName = "command",
                InputTypeNamePattern = "{MutationName}Command",
                PayloadTypeNamePattern = "{MutationName}Dto",
                PayloadErrorsFieldName = "errors",
                PayloadErrorTypeNamePattern = "{MutationName}Error"
            })
            .AddErrorInterfaceType<IUserError>()
            // Error handling and logging
            .AddErrorFilter<GraphQLErrorFilter>()
            // Query capabilities
            .AddProjections()
            .AddFiltering()
            .AddSorting()
            .AddPagingArguments()
            .ModifyPagingOptions(options =>
            {
                options.MaxPageSize = 10;
                options.DefaultPageSize = 10;
                options.IncludeTotalCount = true;
            })
            .AddMaxExecutionDepthRule(4)
            //.AddGlobalObjectIdentification()
            .AddDataLoader<CustomBooksByAuthorIdsDataLoader>()
            // Data store
            .RegisterDbContextFactory<CatalogDbContext>()
            .AddInMemorySubscriptions()
            // Performance
            .InitializeOnStartup(
                warmup: async (executor, cancellationToken) => { await executor.ExecuteAsync("{ __typename }", cancellationToken); });

        services.AddScoped<DataLoaderOptions>(sp => new DataLoaderOptions
        {
            MaxBatchSize = 2
        });
    }

    private static void ConfigureInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Queryable - Hot chocolate will dispose of the DbContext.
        services.AddScoped<IBookReadRepository, BookReadRepository>();
        services.AddScoped<IAuthorReadRepository, AuthorReadRepository>();

        // Command
        services.AddScoped<IBookWriteRepository, BookWriteRepository>();
        services.AddScoped<IAuthorWriteRepository, AuthorWriteRepository>();

        services.AddScoped<ScopedService>();

        services.AddDbContextPool<CatalogDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
        services.AddPooledDbContextFactory<CatalogDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }
}