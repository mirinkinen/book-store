using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Cataloging.Infrastructure.Database;
using Cataloging.Infrastructure.Queries;
using Cataloging.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cataloging.Infrastructure;

public static class ServiceRegistrar
{
    public static void RegisterInfrastructureServices(IServiceCollection services)
    {
        services.AddDbContext<CatalogDbContext>(
            dbContextOptions =>
            {
    #pragma warning disable CS8604 // Possible null reference argument.
                dbContextOptions.UseSqlServer("Data Source=(localdb)\\BookStore;Initial Catalog=BookStore;Integrated Security=True");            

    #pragma warning restore CS8604 // Possible null reference argument.
            });

        services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
    }
}