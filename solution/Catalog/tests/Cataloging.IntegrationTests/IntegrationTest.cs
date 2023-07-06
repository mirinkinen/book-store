using Cataloging.Infrastructure.Database;
using Cataloging.IntegrationTests.Fakes;
using Cataloging.MockDataSeeder;
using Common.Application.Authentication;
using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace Cataloging.IntegrationTests;

public class IntegrationTest : IAsyncLifetime
{
    private const string _instanceName = "BookStoreTest";
    private static readonly SqlLocalDbApi _sqlLoccalDbApi = new();
    private static bool _databaseStarted;

    protected ApiTestWebApplicationFactory Factory { get; } = new();

    protected IHost Host => Factory.Host;

    protected ITestOutputHelper Output { get; }

    protected IUserService UserService => Factory.UserService;

    public IntegrationTest(ITestOutputHelper output)
    {
        Output = output;
    }

    public async Task DisposeAsync()
    {
        await DeleteDatabase();
    }

    public async Task InitializeAsync()
    {
        StartTestDatabaseInstance();

        await CreateAndSeedDatabase();
    }

    [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities")]
    private static void DropLeftoverDatabases(ISqlLocalDbInstanceInfo instance)
    {
        using var connection = instance.CreateConnection();
        connection.Open();
        connection.ChangeDatabase("master");
        using var getDatabasesCommand = new SqlCommand("SELECT * FROM sys.databases WHERE name LIKE 'BookStoreTest%'", connection);
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

    private async Task CreateAndSeedDatabase()
    {
        var dbOptions = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer(Factory.ConnectionString)
            .Options;

        using var dbContext = new CatalogDbContext(dbOptions, new FakeUserService());
        await dbContext.Database.EnsureCreatedAsync();
        await DataSeeder.SeedDataAsync(dbContext);
    }

    private async Task DeleteDatabase()
    {
        var dbOptions = new DbContextOptionsBuilder<CatalogDbContext>()
              .UseSqlServer(Factory.ConnectionString)
              .Options;

        using var dbContext = new CatalogDbContext(dbOptions, UserService);

        await dbContext.Database.EnsureDeletedAsync();
    }
}