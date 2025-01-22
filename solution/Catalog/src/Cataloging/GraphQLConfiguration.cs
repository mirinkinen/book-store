
namespace Cataloging;

public static class GraphQLConfiguration
{
    internal static void AddGraphQLConfiguration(this WebApplicationBuilder builder)
    {
        builder.AddGraphQL().AddTypes();
    }
}