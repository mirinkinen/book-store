using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;

namespace API.IntegrationTests;

public class SchemaTests
{
    [Fact]
    public async Task VerifySchema()
    {
        var configuration = TestConfigurationHelper.CreateTestConfiguration();

        var schema = await TestConfigurationHelper.ConfigureTestServices(configuration)
            .AddGraphQLServer()
            .BuildSchemaAsync(cancellationToken: TestContext.Current.CancellationToken);

        var s = schema.ToString();

        // Assert
        await Verify(s);
    }
}