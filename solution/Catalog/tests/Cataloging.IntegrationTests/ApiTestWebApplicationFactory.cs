using Cataloging.Api;
using Cataloging.Application.Services;
using Cataloging.Infrastructure.Database;
using Cataloging.IntegrationTests.Fakes;
using MartinCostello.SqlLocalDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cataloging.IntegrationTests;

public class ApiTestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string _instanceName = "BookStoreTest";
    private static readonly SqlLocalDbApi _sqlLoccalDbApi = new();

    public Action<IServiceCollection> ConfigureServices { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            // Remove the real database dependency with test instance.
            ReplaceDatabase(services);

            // Replace IUserService.
            var userServiceDescriptor = services.Single(d => d.ImplementationType == typeof(UserService));
            services.Remove(userServiceDescriptor);
            services.AddScoped<IUserService, FakeUserService>();

            ConfigureServices?.Invoke(services);
        });
    }

    private static void ReplaceDatabase(IServiceCollection services)
    {
        var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<CatalogDbContext>));
        services.Remove(dbContextDescriptor);

        StartTestDatabaseInstance();

        var dbName = $"BookStoreTest_{Guid.NewGuid():N}";
        var connectionString = $"Data Source=(localdb)\\{_instanceName};Initial Catalog={dbName};Integrated Security=True";

        services.AddDbContext<CatalogDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);
        });
    }

    private static void StartTestDatabaseInstance()
    {
        // Prevent multiple threads from creating the database.
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