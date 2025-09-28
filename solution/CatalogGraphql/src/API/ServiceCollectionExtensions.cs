using API.Operations;
using Application.AuthorCommands.CreateAuthor;
using Application.AuthorQueries;
using Application.AuthorQueries.GetAuthors;
using Application.BookQueries;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Monitor.OpenTelemetry.Exporter;
using Common.Domain;
using Domain;
using HotChocolate.Diagnostics;
using HotChocolate.Execution;
using Infra.Data;
using Infra.DataLoaders;
using Infra.Repositories;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace API;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures services for the application.
    /// </summary>
    /// <remarks>This method is also called in integration tests. Place everything here that should be used both in production and
    /// integration tests.</remarks>
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureGraphql();
        services.ConfigureInfraServices(configuration);

        // Configure GraphQL with a single Query type containing all operations
        services.AddMediatR(conf => { conf.RegisterServicesFromAssemblyContaining<CreateAuthorHandler>(); });

        return services;
    }

    public static void ConfigureLogging(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Application Insights telemetry
        services.AddApplicationInsightsTelemetry(configuration);

        // Configure telemetry initializer
        services.AddSingleton<ITelemetryInitializer, GraphQLTelemetryInitializer>();

        var connectionString = configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");

        services.AddOpenTelemetry()
            .WithTracing(b =>
            {
                b.AddHttpClientInstrumentation();
                b.AddAspNetCoreInstrumentation();
                b.AddHotChocolateInstrumentation();
                b.AddSource(nameof(BookQueries));
            }).WithMetrics(b =>
            {
                b.AddHttpClientInstrumentation();
                b.AddAspNetCoreInstrumentation();
            }).UseAzureMonitor(c =>
            {
                c.ConnectionString = connectionString;
                c.EnableLiveMetrics = true;
            });
    }

    private static void ConfigureGraphql(this IServiceCollection services)
    {
        services.AddGraphQLServer()
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
            .AddInstrumentation(options => { options.Scopes = ActivityScopes.All; })
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