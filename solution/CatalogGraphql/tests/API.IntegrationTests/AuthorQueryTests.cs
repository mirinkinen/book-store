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
    public async Task Get_authors()
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
    public async Task Get_author_by_id()
    {
        // Arrange
        var query = """
                    query {
                      authorById(id: "8E6A9434-87F5-46B2-A6C3-522DC35D8EEF") {    
                        __typename
                        ... on AuthorNode {
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
    public async Task Get_first_2_authors()
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
    
    [Fact]
    public async Task Get_first_2_authors_with_first_2_books()
    {
        // Arrange
        var query = """
                    query {
                    authors(first: 2) {
                      nodes {
                        __typename
                        id
                        firstName      
                        books(first: 2) {
                          nodes {
                            id
                            title
                          }
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