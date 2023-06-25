using Books.Api.Tests;
using Books.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Books.IntegrationTests;

public class DatabaseTest : IAsyncLifetime
{
    protected ApiTestWebApplicationFactory Factory { get; private set; } = new();

    public async Task DisposeAsync()
    {
        await DeleteDatabase();
    }

    public async Task InitializeAsync()
    {
        await CreateAndSeedDatabase();
    }

    private async Task CreateAndSeedDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<BooksDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        await DataSeeder.SeedDataAsync(dbContext);
    }

    private async Task DeleteDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<BooksDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }
}