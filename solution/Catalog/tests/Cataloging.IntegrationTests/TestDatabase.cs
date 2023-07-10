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
    private static readonly object _cleanerLock = new();
    private const string _sqlInstanceName = "BookStoreTest";
    private const string _sqlDatabasePrefix = "Books";

    public string Name { get; }

    public string ConnectionString { get; }


    public TestDatabase()
    {
        CleanLeftoverDatabases();

        Name = $"{_sqlDatabasePrefix}-{Guid.NewGuid():D}";
        ConnectionString =
            $"Data Source=(localdb)\\{_sqlInstanceName};Initial Catalog={Name};Integrated Security=True";

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

            using SqlLocalDbApi sqlLocalDbApi = new();
            var instance = sqlLocalDbApi.GetOrCreateInstance(_sqlInstanceName);

            if (!instance.IsRunning)
            {
                sqlLocalDbApi.StartInstance(instance.Name);
                // Starting an instance doesn't seem to effect the current instance object.
                // Therefore, the started instance needs to be fetched again.
                instance = sqlLocalDbApi.GetOrCreateInstance(_sqlInstanceName);
            }

            using var connection = instance.CreateConnection();
            connection.Open();
            connection.ChangeDatabase("master");

            using var getDatabasesCommand =
                new SqlCommand($"SELECT * FROM sys.databases WHERE name LIKE '{_sqlDatabasePrefix}%'", connection);
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