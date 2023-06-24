using Books.Application.Services;
using Books.Domain.Authors;
using Books.Infrastructure.Database;
using Books.Infrastructure.Queries;
using Books.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Infrastructure;

public static class ServiceRegistrar
{
    public static void RegisterInfrastructureServices(IServiceCollection services)
    {
        services.AddDbContext<BooksDbContext>(dbContextOptions =>
        {
#pragma warning disable CS8604 // Possible null reference argument.
            dbContextOptions.UseSqlServer("Data Source=(localdb)\\BookStore;Initial Catalog=BookStore;Integrated Security=True");

#pragma warning restore CS8604 // Possible null reference argument.
        });

        services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
    }
}