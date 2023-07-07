using System.Diagnostics.CodeAnalysis;
using Cataloging.Api;
using Cataloging.IntegrationTests.Fakes;
using Common.Application.Authentication;
using MartinCostello.SqlLocalDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cataloging.IntegrationTests;

public class IntegrationWebApplicationFactory : WebApplicationFactory<Program>
{
    public Action<IServiceCollection>? ConfigureServices { get; set; }

    public IUserService UserService { get; } = new FakeUserService();

    public TestDatabase TestDatabase { get; }

    public string ConnectionString { get; private set; }

    public IntegrationWebApplicationFactory()
    {
        TestDatabase = new();
    }

    protected override void Dispose(bool disposing)
    {
        TestDatabase.Dispose();
        
        base.Dispose(disposing);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

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

    private void OverrideAppSettings(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:CatalogConnectionString", TestDatabase.ConnectionString);
    }

    [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities")]
    private static void DropLeftoverDatabases(ISqlLocalDbInstanceInfo instance)
    {
        using var connection = instance.CreateConnection();
        connection.Open();
        connection.ChangeDatabase("master");
        using var getDatabasesCommand =
            new SqlCommand("SELECT * FROM sys.databases WHERE name LIKE 'BookStoreTest%'", connection);
        using var reader = getDatabasesCommand.ExecuteReader();
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
            catch (SqlException)
            {
            }
        }
    }
}