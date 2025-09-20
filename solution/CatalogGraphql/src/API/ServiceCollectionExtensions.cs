using API.Operations;
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
            .AddQueryType()
            .AddMutationType()
            .AddSubscriptionType<Subscriptions>()
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
            // Relay convention
            .AddGlobalObjectIdentification()
            .AddProjections()
            // Data store
            .RegisterDbContextFactory<CatalogDbContext>()
            .AddInMemorySubscriptions();
    }

    private static void ConfigureInfraServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IBookWriteRepository, BookWriteWriteRepository>();
        services.AddScoped<IAuthorWriteRepository, AuthorWriteRepository>();
        services.AddSingleton<IReadRepository<Author>, ReadRepository<Author>>();
        services.AddSingleton<IReadRepository<Book>, ReadRepository<Book>>();

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