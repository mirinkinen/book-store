using Cataloging.Infrastructure.Database;
using Cataloging.MockDataSeeder;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Cataloging.IntegrationTests;

public class IntegrationTest : IAsyncLifetime
{
    protected ApiTestWebApplicationFactory Factory { get; private set; } = new();

    protected ITestOutputHelper Output { get; }

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
        await CreateAndSeedDatabase();
    }

    private async Task CreateAndSeedDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<CatalogDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        await DataSeeder.SeedDataAsync(dbContext);
    }

    private async Task DeleteDatabase()
    {
        using var scope = Factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<CatalogDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }
}