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
    public async Task GetBooks_ReturnsBooks()
    {
        // Arrange
        var query = """
                    query {
                      books(order: [{ title: ASC }]) {
                        nodes {
                          __typename
                          id
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
    public async Task GetBookById_ReturnsBook()
    {
        // Arrange
        var query = """
                    query {
                       bookById(id: "6F6D9786-074C-4828-8DDD-5852A9530203") {
                        ... on BookNode {
                          id
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
    public async Task GetBooks_First2_Returns2Books()
    {
        // Arrange
        var query = """
                    query {
                      books(first: 2, order: [{ id: ASC }]) {
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