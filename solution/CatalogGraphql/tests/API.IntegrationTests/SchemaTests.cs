using HotChocolate.Execution;
using Infra.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.IntegrationTests;

public class SchemaTests
{
    [Fact]
    public async Task VerifySchema()
    {
        var inMemoryConfiguration = new ConfigurationBuilder().Build();
        
        var schema = await new ServiceCollection()
            .RegisterServices(inMemoryConfiguration)
            .AddGraphQLServer()
            .AddQueryType()
            .AddMutationType()
            .AddTypes()
            .RegisterDbContextFactory<CatalogDbContext>()
            .BuildSchemaAsync(cancellationToken: TestContext.Current.CancellationToken);

        var s = schema.ToString();

        // Assert
        await Verify(s);
    }
}