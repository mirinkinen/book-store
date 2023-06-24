using Books.Api;
using Books.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Books.IntegrationTests;

public static class TestDataSeeder
{
    public static async Task SeedTestData(WebApplicationFactory<Program> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<BooksDbContext>();
        dbContext.Database.EnsureCreated();

        await DataSeeder.SeedData(dbContext);
    }

    public static void DropTestDatabse(WebApplicationFactory<Program> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<BooksDbContext>();
        dbContext.Database.EnsureDeleted();
    }
}