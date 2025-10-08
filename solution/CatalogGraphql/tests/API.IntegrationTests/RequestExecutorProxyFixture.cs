using HotChocolate;
using HotChocolate.Execution;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using TestData;

namespace API.IntegrationTests;

public class RequestExecutorProxyFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _sqlContainer;


    public RequestExecutorProxyFixture()
    {
        _sqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("YourStrong@Passw0rd")
            .WithCleanUp(true)
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        await _sqlContainer.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _sqlContainer.StopAsync();
        await _sqlContainer.DisposeAsync();
    }

    public async Task<IExecutionResult> ExecuteOperationAsync(string query, Dictionary<string, object?>? variables = null)
    {
        var executor = await GetRequestExecutor();

        OperationRequestBuilder builder = new();
        builder.SetDocument(query);
        builder.AddVariableValues(variables!);
        var operation = builder.Build();

        // Act
        var result = await executor.ExecuteAsync(operation, cancellationToken: TestContext.Current.CancellationToken);
        return result;
    }

    private async Task<RequestExecutorProxy> GetRequestExecutor()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", _sqlContainer.GetConnectionString() },
            })
            .Build();

        var serviceProvider = TestConfigurationHelper.ConfigureTestServices(configuration)
            .AddSingleton(sp =>
                new RequestExecutorProxy(sp.GetRequiredService<IRequestExecutorResolver>(), Schema.DefaultName))
            .BuildServiceProvider();

        var requestExecutor = serviceProvider.GetRequiredService<RequestExecutorProxy>();

        using var scope = serviceProvider.CreateScope();
        var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<CatalogDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Database.EnsureCreatedAsync();
        await DataSeeder.SeedDataAsync(dbContext);

        return requestExecutor;
    }
}