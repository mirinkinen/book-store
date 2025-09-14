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

        var result = await _requestExecutor.ExecuteOperationAsync(query);

        // Assert
        var json = result.ToJson();
        await VerifyJson(json);
    }
}