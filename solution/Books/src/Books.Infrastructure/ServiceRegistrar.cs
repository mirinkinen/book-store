using Books.Application;
using Books.Infrastructure.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Infrastructure;

public static class ServiceRegistrar
{
    private static SqliteConnection? _connection;

    public static void RegisterInfrastructureServices(IServiceCollection services)
    {
        services.AddDbContext<BooksDbContext>(dbContextOptions =>
        {
#pragma warning disable CS8604 // Possible null reference argument.
            dbContextOptions.UseSqlite(_connection);

#pragma warning restore CS8604 // Possible null reference argument.
        });

        services.AddDbContext<AuditBooksDbContext>(dbContextOptions =>
        {
#pragma warning disable CS8604 // Possible null reference argument.
            dbContextOptions.UseSqlite(_connection);

#pragma warning restore CS8604 // Possible null reference argument.
        });

        services.AddScoped<IQueryAuthorizer, QueryAuthorizer>();
    }

    public static async Task InitializeDatabase(IServiceProvider services)
    {
        // Keep connection open or the in-memory database will be gone.
        _connection = new SqliteConnection("datasource=:memory:");
        await _connection.OpenAsync();

        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BooksDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
        await DataSeeder.SeedData(dbContext);
    }
}