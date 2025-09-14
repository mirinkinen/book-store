using Testcontainers.MsSql;

namespace API.IntegrationTests;

public class TestContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer;

    public TestContainerFixture()
    {
        _sqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("YourStrong@Passw0rd")
            .WithCleanUp(true)
            .Build();
    }

    public string ConnectionString => _sqlContainer.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        await _sqlContainer.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _sqlContainer.StopAsync();
        await _sqlContainer.DisposeAsync();
    }
}
