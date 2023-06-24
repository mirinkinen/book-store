using Books.Infrastructure.Database;
using MartinCostello.SqlLocalDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Books.Api.Tests;

public class ApiTestWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly string _instanceName = "BookStoreTest";
    private static readonly SqlLocalDbApi _sqlLoccalDbApi = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            // Remove the real database connection
            var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<BooksDbContext>));
            services.Remove(dbContextDescriptor);

            StartTestDatabaseInstance();

            var dbName = $"BookStoreTest_{Guid.NewGuid():N}";
            var connectionString = $"Data Source=(localdb)\\{_instanceName};Initial Catalog={dbName};Integrated Security=True";

            services.AddDbContext<BooksDbContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        });
    }

    private static void StartTestDatabaseInstance()
    {
        lock (_sqlLoccalDbApi)
        {
            ISqlLocalDbInstanceInfo instance = _sqlLoccalDbApi.GetOrCreateInstance(_instanceName);
            ISqlLocalDbInstanceManager manager = instance.Manage();

            if (!instance.IsRunning)
            {
                manager.Start();
            }
        }
    }
}