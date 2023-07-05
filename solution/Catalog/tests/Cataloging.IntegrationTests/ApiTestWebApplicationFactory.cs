using Cataloging.Api;
using Cataloging.IntegrationTests.Fakes;
using Common.Application.Authentication;
using MartinCostello.SqlLocalDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.IntegrationTests;

public class ApiTestWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string _instanceName = "BookStoreTest";
    private static readonly SqlLocalDbApi _sqlLoccalDbApi = new();
    private static bool _databaseStarted;

    public Action<IServiceCollection>? ConfigureServices { get; set; }

    public IUserService UserService { get; } = new FakeUserService();
    public IHost Host { get; private set; }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        Host = base.CreateHost(builder);
        return Host;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        StartTestDatabaseInstance();
        OverrideAppSettings(builder);

        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            // Replace IUserService.
            var userServiceDescriptor = services.Single(d => d.ImplementationType == typeof(UserService));
            services.Remove(userServiceDescriptor);
            services.AddScoped<IUserService>(sp => UserService);

            ConfigureServices?.Invoke(services);
        });
    }

    private static void OverrideAppSettings(IWebHostBuilder builder)
    {
        var dbName = $"BookStoreTest_{Guid.NewGuid():N}";
        var connectionString = $"Data Source=(localdb)\\BookStore;Initial Catalog={dbName};Integrated Security=True";
        builder.UseSetting("ConnectionStrings:CatalogConnectionString", connectionString);
    }

    [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities")]
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
            // SQLExceptions can happen if database files are not found. Don't care about those scenarios.
            catch (SqlException) { }
        }
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
}