using Books.Api.Tests;

namespace Books.IntegrationTests;

public class DatabaseTest : IDisposable, IAsyncLifetime
{
    protected ApiTestWebApplicationFactory Factory { get; private set; } = new();

    public void Dispose()
    {
        Factory?.Dispose();
    }

    public Task DisposeAsync()
    {
        TestDataSeeder.DropTestDatabse(Factory);

        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return TestDataSeeder.SeedTestData(Factory);
    }
}