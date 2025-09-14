using Application.AuthorQueries.GetAuthor;
using AwesomeAssertions;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Execution.Configuration;
using Infra.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.IntegrationTests;

public class AuthorQueryTests
{
    [Fact]
    public async Task GetAuthor_WithValidId_ReturnsAuthor()
    {
        // Arrange
        var query = """
                    query {
                      author(input: { id: "8E6A9434-87F5-46B2-A6C3-522DC35D8EEF" }) {
                        id
                        firstName
                        lastName
                        birthdate
                        organizationId
                      }
                    }
                    """;
        
        var executor = GetRequestExecutor();

        // Act
        var result = await executor.ExecuteRequestAsync(query, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
    }

    private static IRequestExecutorBuilder GetRequestExecutor()
    {
        var inMemoryConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"ConnectionStrings:DefaultConnection", "Data Source=InMemoryDatabase"}
            })
            .Build();
        
        return new ServiceCollection()
            .RegisterServices(inMemoryConfiguration)
            .AddGraphQLServer()
            .AddQueryType()
            .AddMutationType()
            .AddTypes()
            .RegisterDbContextFactory<CatalogDbContext>();
    }
}
