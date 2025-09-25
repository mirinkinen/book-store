using Application;
using Application.AuthorCommands.CreateAuthor;
using Application.AuthorQueries;
using Application.AuthorQueries.GetAuthors;
using Application.BookQueries;
using Common.Domain;
using Domain;
using HotChocolate.Execution;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureGraphql();
        services.ConfigureInfraServices(configuration);

        // Configure GraphQL with a single Query type containing all operations
        services.AddMediatR(conf => { conf.RegisterServicesFromAssemblyContaining<CreateAuthorHandler>(); });

        return services;
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
            //.AddGlobalObjectIdentification()
            // Data store
            .RegisterDbContextFactory<CatalogDbContext>()
            .AddInMemorySubscriptions()
            // Performance
            .InitializeOnStartup(
                warmup: async (executor, cancellationToken) => { await executor.ExecuteAsync("{ __typename }", cancellationToken); });
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