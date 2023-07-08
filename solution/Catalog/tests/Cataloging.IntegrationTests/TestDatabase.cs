using System.Diagnostics.CodeAnalysis;
using Cataloging.Infrastructure.Database;
using Cataloging.IntegrationTests.Fakes;
using Cataloging.MockDataSeeder;
using MartinCostello.SqlLocalDb;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Cataloging.IntegrationTests;

[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities")]
public sealed class TestDatabase : IAsyncDisposable
{
    private static bool _leftOverDatabasesCleaned;
    private static object _cleanerLock = new();

    public string Name { get; }

    public string ConnectionString { get; }


    public TestDatabase()
    {
        CleanLeftoverDatabases();

        Name = $"BookStoreTest-{Guid.NewGuid():D}";
        ConnectionString =
            $"Data Source=(localdb)\\BookStoreTest;Initial Catalog={Name};Integrated Security=True";

        CreateAndSeedDatabase().Wait();
    }

    private static void CleanLeftoverDatabases()
    {
        // Allow only one thread at a time.
        lock (_cleanerLock)
        {
            // Run only once per test run.
            if (_leftOverDatabasesCleaned)
            {
                return;
            }

            _leftOverDatabasesCleaned = true;

            using SqlLocalDbApi sqlLoccalDbApi = new();
            var instance = sqlLoccalDbApi.GetOrCreateInstance("BookStoreTest");

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
                    using var dropCommand = new SqlCommand($"DROP DATABASE [{database}]", connection);
                    dropCommand.ExecuteNonQuery();
                }
                // SQLExceptions can happen if database files are not found. Don't care about those scenarios.
                catch (SqlException)
                {
                }
            }
        }
    }


    private async Task CreateAndSeedDatabase()
    {
        var dbOptions = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        await using var dbContext = new CatalogDbContext(dbOptions, new FakeUserService());
        await dbContext.Database.MigrateAsync();
        await DataSeeder.SeedDataAsync(dbContext);
    }

    private async Task DeleteDatabase()
    {
        var dbOptions = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        await using var dbContext = new CatalogDbContext(dbOptions, new FakeUserService());
        await dbContext.Database.EnsureDeletedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await DeleteDatabase();
    }
}