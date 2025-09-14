using Application.AuthorMutations.CreateAuthor;
using Application.Repositories;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureEFCore(configuration);
        services.ConfigureInfraServices();
        
        // Configure GraphQL with a single Query type containing all operations
        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblyContaining<CreateAuthorHandler>();
        });
        
        return services;
    }
    
    internal static IServiceCollection ConfigureInfraServices(this IServiceCollection services)
    {
        services.AddSingleton<IBookRepository, BookRepository>();
        services.AddSingleton<IAuthorRepository, AuthorRepository>();

        return services;
    }
    
    internal static void ConfigureEFCore(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContextFactory<CatalogDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });
    }
}