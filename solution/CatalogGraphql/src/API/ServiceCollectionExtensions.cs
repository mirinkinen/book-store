using Application.AuthorCommands.CreateAuthor;
using Application.AuthorQueries.GetAuthors;
using Common.Domain;
using Domain;
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
            .ModifyPagingOptions(options =>
            {
                options.MaxPageSize = 10;
                options.DefaultPageSize = 10;
                options.IncludeTotalCount = true;
            })
            .AddDbContextCursorPagingProvider()
            // Data store
            .RegisterDbContextFactory<CatalogDbContext>()
            .AddInMemorySubscriptions();
    }

    private static void ConfigureInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Query
        services.AddSingleton<IReadRepository<Author>, ReadRepository<Author>>();
        services.AddSingleton<IReadRepository<Book>, ReadRepository<Book>>();

        // Queryable - Hot chocolate will dispose of the DbContext.
        services.AddScoped<IQueryRepository<Author>, QueryRepository<Author>>();
        services.AddScoped<IQueryRepository<Book>, QueryRepository<Book>>();

        // Command
        services.AddScoped<IBookWriteRepository, BookWriteWriteRepository>();
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