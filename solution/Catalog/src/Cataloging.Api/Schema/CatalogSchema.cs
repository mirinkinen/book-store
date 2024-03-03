using Cataloging.Api.Schema.Types;

namespace Cataloging.Api.Schema;

public class CatalogSchema : GraphQL.Types.Schema
{
    public CatalogSchema(IServiceProvider serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<CatalogQuery>();
    }
}