using System.Diagnostics.CodeAnalysis;
using Cataloging.Infrastructure.Database;
using Cataloging.Infrastructure.Database.Setup;
using Cataloging.IntegrationTests.Fakes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Cataloging.IntegrationTests;

[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities")]
public sealed class TestDatabase : IAsyncDisposable
{
    private static bool _leftOverDatabasesCleaned;
    private static readonly object _cleanerLock = new();
    private const string _sqlDatabasePrefix = "BookCatalogTest";

    public string Name { get; }

    public string ConnectionString { get; }

    public TestDatabase()
    {
        CleanLeftoverDatabases();

        Name = $"{_sqlDatabasePrefix}-{Guid.NewGuid():D}";
        ConnectionString =
            $"Data Source=localhost;Initial Catalog={Name};User ID=sa;Trust Server Certificate=True;Authentication=SqlPassword;Password=P@55w0rd";
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

            var masterConnectionString =
                "Data Source=localhost;Initial Catalog=master;User ID=sa;Trust Server Certificate=True;Authentication=SqlPassword;Password=P@55w0rd";
            using SqlConnection connection = new(masterConnectionString);
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

    public async Task CreateAndSeedDatabase()
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