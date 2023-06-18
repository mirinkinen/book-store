using Books.Application;
using Books.Infrastructure.Database;
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

        // Enable audit logging for all entities.
        Audit.EntityFramework.Configuration
            .Setup()
            .ForContext<BooksDbContext>(config => config
                .IncludeEntityObjects()
                .AuditEventType("{context}:{database}"));

        services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
    }

    public static async Task InitializeDatabase(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BooksDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await DataSeeder.SeedData(dbContext);
    }
}