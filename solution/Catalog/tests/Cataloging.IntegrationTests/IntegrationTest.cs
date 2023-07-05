using Cataloging.Infrastructure.Database;
using Cataloging.MockDataSeeder;
using Common.Application.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace Cataloging.IntegrationTests;

public class IntegrationTest : IAsyncLifetime
{
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