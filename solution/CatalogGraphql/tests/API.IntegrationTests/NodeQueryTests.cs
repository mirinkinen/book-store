using HotChocolate;

namespace API.IntegrationTests;

public class NodeQueryTests : IClassFixture<RequestExecutorProxyFixture>
{
    private readonly RequestExecutorProxyFixture _requestExecutor;

    public NodeQueryTests(RequestExecutorProxyFixture requestExecutor)
    {
        _requestExecutor = requestExecutor;
    }

    [Fact]
    public async Task Get_nodes_by_ids()
    {
        // Arrange
        var query = """
                    query {
                      nodes(
                        ids: ["Qm9vazqGl21vTAcoSI3dWFKpUwID", "QXV0aG9yOk+MC1Lxcs5OseKN0dyjR2o="]
                      ) {
                        ... on Book {
                          title
                          datePublished
                        }
                        ... on Author {
                          firstName
                          lastName
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