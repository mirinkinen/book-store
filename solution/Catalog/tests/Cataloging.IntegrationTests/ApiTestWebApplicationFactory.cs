using Cataloging.Api;
using Cataloging.Infrastructure.Database;
using Cataloging.IntegrationTests.Fakes;
using MartinCostello.SqlLocalDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Authentication;
using System.Data.Common;

namespace Cataloging.IntegrationTests;

public class ApiTestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string _instanceName = "BookStoreTest";
    private static readonly SqlLocalDbApi _sqlLoccalDbApi = new();
    private static bool _databaseStarted;

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
        StartTestDatabaseInstance();

        var dbContextDescriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<CatalogDbContext>));
        services.Remove(dbContextDescriptor);
        var dbContext = services.Single(d => d.ServiceType == typeof(CatalogDbContext));
        services.Remove(dbContext);

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
            // Do this only once.
            if (_databaseStarted)
            {
                return;
            }

            _databaseStarted = true;

            ISqlLocalDbInstanceInfo instance = _sqlLoccalDbApi.GetOrCreateInstance(_instanceName);
            ISqlLocalDbInstanceManager manager = instance.Manage();

            if (!instance.IsRunning)
            {
                manager.Start();
            }

            DropLeftoverDatabases(instance);
        }
    }

    private static void DropLeftoverDatabases(ISqlLocalDbInstanceInfo instance)
    {
        using var connection = instance.CreateConnection();
        connection.Open();
        connection.ChangeDatabase("master");
        using var getDatabasesCommand = new SqlCommand("SELECT * FROM sys.databases WHERE name LIKE 'BookStoreTest%'", connection);
        var reader = getDatabasesCommand.ExecuteReader();
        var databases = new List<string>();

        while (reader.Read())
        {
            databases.Add(reader.GetString(0));
        }

        reader.Close();

        foreach (var database in databases)
        {
            try
            {
                using var dropCommand = new SqlCommand($"DROP DATABASE {database}", connection);
                dropCommand.ExecuteNonQuery();
            }
            // Don't care about failures.
            catch { }
        }
    }
}