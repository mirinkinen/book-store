using HotChocolate;

namespace API.IntegrationTests;

public class AuthorQueryTests : IClassFixture<RequestExecutorProxyFixture>
{
    private readonly RequestExecutorProxyFixture _requestExecutor;

    public AuthorQueryTests(RequestExecutorProxyFixture requestExecutor)
    {
        _requestExecutor = requestExecutor;
    }

    [Fact]
    public async Task GetAuthors_ReturnsAuthors()
    {
        // Arrange
        var query = """
                    query {
                      authors(order: [{ id: ASC }]) {
                        nodes {
                          __typename
                          id
                          firstName
                          lastName
                          birthdate
                          organizationId
                        }
                      }
                    }
                    
                    """;

        var result = await _requestExecutor.ExecuteOperationAsync(query);

        // Assert
        var json = result.ToJson();
        await VerifyJson(json);
    }
    
    [Fact]
    public async Task GetAuthorById_ReturnsAuthor()
    {
        // Arrange
        var query = """
                    query {
                      authorById(id: "QXV0aG9yOjSUao71h7JGpsNSLcNdju8=") {    
                        __typename
                        ... on Author {
                          id
                          firstName
                          lastName
                          birthdate
                          organizationId
                        }
                      }
                    }
                    """;

        var result = await _requestExecutor.ExecuteOperationAsync(query);

        // Assert
        var json = result.ToJson();
        await VerifyJson(json);
    }
    
    [Fact]
    public async Task GetAuthors_First2_Returns2Authors()
    {
        // Arrange
        var query = """
                    query {
                      authors(first: 2, order: [{ id: ASC }]) {
                        nodes {
                          id
                        }
                      }
                    }
                    
                    """;

        var result = await _requestExecutor.ExecuteOperationAsync(query);

        // Assert
        var json = result.ToJson();
        await VerifyJson(json);
    }
}