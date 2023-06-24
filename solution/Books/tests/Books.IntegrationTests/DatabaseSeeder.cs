using Books.Api;
using Books.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Books.IntegrationTests;

public static class TestDataSeeder
{
    public static Task SeedTestData(WebApplicationFactory<Program> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        using var scope = factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<BooksDbContext>();

        return DataSeeder.SeedData(db);
    }
}