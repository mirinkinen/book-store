using Application.AuthorCommands.CreateAuthor;
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
        services.ConfigureEFCore(configuration);
        services.ConfigureInfraServices();

        // Configure GraphQL with a single Query type containing all operations
        services.AddMediatR(conf => { conf.RegisterServicesFromAssemblyContaining<CreateAuthorHandler>(); });

        return services;
    }

    internal static void ConfigureInfraServices(this IServiceCollection services)
    {
        services.AddSingleton<IBookRepository, BookRepository>();
        services.AddSingleton<IAuthorRepository, AuthorRepository>();
    }

    internal static void ConfigureEFCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<CatalogDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }

    internal static void ConfigureGraphql(this IServiceCollection services)
    {
        services.AddGraphQLServer()
            .AddGraphQLServer()
            .AddQueryType()
            .AddMutationType()
            .AddTypes()
            .RegisterDbContextFactory<CatalogDbContext>();
    }
}