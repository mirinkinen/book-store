using HotChocolate;

namespace API.IntegrationTests;

public class OrderItemQueryTests : IClassFixture<RequestExecutorProxyFixture>
{
    private readonly RequestExecutorProxyFixture _requestExecutor;

    public OrderItemQueryTests(RequestExecutorProxyFixture requestExecutor)
    {
        _requestExecutor = requestExecutor;
    }

    [Fact]
    public async Task Get_order_items()
    {
        // Arrange
        var query = """
                    query {
                      orderItems(order: [{ id: ASC }]) {
                        nodes {
                          __typename
                          productName      
                          quantity
                          unitPrice
                          orderId
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
    public async Task Get_order_item_by_id()
    {
        // Arrange
        var query = """
                    query {
                      node(id: "T3JkZXJJdGVtOr3FJaGOT5RHnDZ25AH7TyQ=") {    
                        __typename
                        ... on OrderItem {
                          productName
                          quantity
                          unitPrice
                          orderId
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
    public async Task Get_first_2_order_items()
    {
        // Arrange
        var query = """
                    query {
                      orderItems(first: 2, order: [{ id: ASC }]) {
                        nodes {
                          productName
                          quantity
                          unitPrice
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
    public async Task Get_order_items_by_product_name_using_variable()
    {
        // Arrange
        var query = """
                    query($productName: String!) {
                      orderItems(where:  {
                         productName:  {
                            contains: $productName
                         }
                      }) {
                        nodes {
                          productName      
                          quantity
                          unitPrice
                        }
                      }
                    }
                    """;

        var variables = new Dictionary<string, object?> { { "productName", "Gatsby" } };

        var result = await _requestExecutor.ExecuteOperationAsync(query, variables);

        // Assert
        var json = result.ToJson();
        await VerifyJson(json);
    }
}