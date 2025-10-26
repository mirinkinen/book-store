using HotChocolate;

namespace API.IntegrationTests;

public class OrderQueryTests : IClassFixture<RequestExecutorProxyFixture>
{
    private readonly RequestExecutorProxyFixture _requestExecutor;

    public OrderQueryTests(RequestExecutorProxyFixture requestExecutor)
    {
        _requestExecutor = requestExecutor;
    }

    [Fact]
    public async Task Get_orders()
    {
        // Arrange
        var query = """
                    query {
                      orders(order: [{ id: ASC }]) {
                        nodes {
                          __typename
                          customerName      
                          orderDate
                          totalAmount
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
    public async Task Get_order_by_id()
    {
        // Arrange
        var query = """
                    query {
                      node(id: "T3JkZXI6EnJKbNNl8CSkoTALoTts3Q==") {    
                        __typename
                        ... on Order {
                          customerName
                          orderDate
                          totalAmount
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
    public async Task Get_first_2_orders()
    {
        // Arrange
        var query = """
                    query {
                      orders(first: 2, order: [{ id: ASC }]) {
                        nodes {
                          customerName
                          orderDate
                          totalAmount
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
    public async Task Get_orders_by_customer_name_using_variable()
    {
        // Arrange
        var query = """
                    query($name: String!) {
                      orders(where:  {
                         customerName:  {
                            contains: $name
                         }
                      }) {
                        nodes {
                          customerName      
                          orderDate
                          totalAmount
                        }
                      }
                    }
                    """;

        var variables = new Dictionary<string, object?> { { "name", "John" } };

        var result = await _requestExecutor.ExecuteOperationAsync(query, variables);

        // Assert
        var json = result.ToJson();
        await VerifyJson(json);
    }
}