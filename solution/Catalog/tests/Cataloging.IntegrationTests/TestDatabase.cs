using Cataloging.Infrastructure.Database;
using Cataloging.Infrastructure.Database.Setup;
using Cataloging.IntegrationTests.Fakes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.IntegrationTests;

[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities")]
public sealed class TestDatabase : IAsyncDisposable
{
    private static bool _leftOverDatabasesCleaned;
    private static readonly SemaphoreSlim _cleanerSemaphore = new(1, 1);
    private const string _sqlDatabasePrefix = "BookCatalogTest";

    public string Name { get; }

    public string ConnectionString { get; }

    public TestDatabase()
    {
        Name = $"{_sqlDatabasePrefix}-{Guid.NewGuid():D}";
        ConnectionString =
            $"Data Source=localhost;Initial Catalog={Name};User ID=sa;Trust Server Certificate=True;Authentication=SqlPassword;Password=P@55w0rd";
    }

    

    public static async Task CleanLeftoverDatabasesAsync()
    {
        await _cleanerSemaphore.WaitAsync();
        try
        {
            // Run only once per test run.
            if (_leftOverDatabasesCleaned)
            {
                return;
            }

            _leftOverDatabasesCleaned = true;

            var masterConnectionString =
                "Data Source=localhost;Initial Catalog=master;User ID=sa;Trust Server Certificate=True;Authentication=SqlPassword;Password=P@55w0rd";
            await using SqlConnection connection = new(masterConnectionString);
            await connection.OpenAsync();
            await connection.ChangeDatabaseAsync("master");

            await using var getDatabasesCommand =
                new SqlCommand($"SELECT * FROM sys.databases WHERE name LIKE '{_sqlDatabasePrefix}%'", connection);
            await using var reader = await getDatabasesCommand.ExecuteReaderAsync();
            var databases = new List<string>();

            while (await reader.ReadAsync())
            {
                databases.Add(reader.GetString(0));
            }

            await reader.CloseAsync();

            foreach (var database in databases)
            {
                try
                {
                    await using var dropCommand = new SqlCommand($"DROP DATABASE [{database}]", connection);
                    await dropCommand.ExecuteNonQueryAsync();
                }
                // SQLExceptions can happen if database files are not found. Don't care about those scenarios.
                catch (SqlException)
                {
                }
            }
        }
        finally
        {
            _cleanerSemaphore.Release();
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