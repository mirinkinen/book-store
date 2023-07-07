using System.Diagnostics.CodeAnalysis;
using Cataloging.Infrastructure.Database;
using Cataloging.IntegrationTests.Fakes;
using Cataloging.MockDataSeeder;
using Microsoft.EntityFrameworkCore;

namespace Cataloging.IntegrationTests;

public sealed class TestDatabase : IDisposable
{
    public string Name { get; }

    public string ConnectionString { get; }


    public TestDatabase()
    {
        Name = $"BookStoreTest-{Guid.NewGuid():D}";
        ConnectionString =
            $"Data Source=(localdb)\\BookStoreTest;Initial Catalog={Name};Integrated Security=True";

        CreateAndSeedDatabase().Wait();
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

        using var dbContext = new CatalogDbContext(dbOptions, new FakeUserService());

        await dbContext.Database.EnsureDeletedAsync();
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            DeleteDatabase().Wait();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}