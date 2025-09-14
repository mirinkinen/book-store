using API.Operations;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Language;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestData;

namespace API.IntegrationTests;

public class AuthorQueryTests : IClassFixture<TestContainerFixture>
{
    private readonly TestContainerFixture _fixture;

    public AuthorQueryTests(TestContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAuthors_ReturnsAuthors()
    {
        // Arrange
        var query = """
                    query {
                      authors {
                        __typename
                        id
                        firstName
                        lastName
                        birthdate
                        organizationId
                      }
                    }
                    """;

        var executor = await GetRequestExecutor();

        OperationRequestBuilder builder = new();
        builder.SetDocument(query);
        var operation = builder.Build();

        // Act
        var result = await executor.ExecuteAsync(operation, cancellationToken: TestContext.Current.CancellationToken);
        
        // Assert
        var json = result.ToJson();
        await VerifyJson(json);
    }

    private async Task<RequestExecutorProxy> GetRequestExecutor()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", _fixture.ConnectionString }
            })
            .Build();

        
        var serviceProvider = new ServiceCollection()
            .RegisterServices(configuration)
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