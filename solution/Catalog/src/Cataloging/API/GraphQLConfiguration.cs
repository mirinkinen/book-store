
using Microsoft.Extensions.DependencyInjection;

namespace Cataloging.API;

public static class GraphQLConfiguration
{
    internal static void AddGraphQLConfiguration(this WebApplicationBuilder builder)
    {
        builder.AddGraphQL().AddTypes();
    }
}