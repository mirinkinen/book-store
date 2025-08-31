using API.Operations;
using Application.Repositories;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API;

public static class ServiceConfigurator
{
    internal static void ConfigureGraphQL(this WebApplicationBuilder builder)
    {
        builder.AddGraphQL()
            .AddGraphQLServer()
            .AddQueryType()
            .AddMutationType()
            .AddTypes()
            .RegisterDbContextFactory<CatalogDbContext>();
    }

    internal static void ConfigureEFCore(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDbContextFactory<CatalogDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });
    }
    
    internal static void ConfigureInfraServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBookRepository, BookRepository>();
        builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();
    }
}