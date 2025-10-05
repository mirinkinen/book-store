using HotChocolate;

namespace API.IntegrationTests;

public class BookQueryTests : IClassFixture<RequestExecutorProxyFixture>
{
    private readonly RequestExecutorProxyFixture _requestExecutor;

    public BookQueryTests(RequestExecutorProxyFixture requestExecutor)
    {
        _requestExecutor = requestExecutor;
    }

    [Fact]
    public async Task Get_books_without_limit_Returns_10_Books()
    {
        // Arrange
        var query = """
                    query {
                      books(order: [{ title: ASC }]) {
                        nodes {
                          __typename
                          title
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
    public async Task Get_book_by_id()
    {
        // Arrange
        var query = """
                    query {
                       bookById(id: "Qm9vazqGl21vTAcoSI3dWFKpUwID") {
                        ... on Book {
                          id
                          title
                        }
                      }
                    }
                    """;

        var result = await _requestExecutor.ExecuteOperationAsync(query);

        // Assert
        var settings = new VerifySettings();
        settings.IgnoreMember("id");
        var json = result.ToJson();
        await VerifyJson(json, settings);
    }
    
    [Fact]
    public async Task Get_first_2_Books()
    {
        // Arrange
        var query = """
                    query {
                      books(first: 2, order: [{ title: ASC }]) {
                        nodes {
                          title
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
    public async Task Get_first_2_books_with_author()
    {
        // Arrange
        var query = @"
            query {
              books(first: 2, order: [{ title: ASC }]) {
                nodes {
                  title
                  authorId
                  author {
                    firstName
                    lastName
                  }
                }
              }
            }
            ";

        // Act
        var result = await _requestExecutor.ExecuteOperationAsync(query);

        // Assert
        var json = result.ToJson();
        await VerifyJson(json);
    }
}