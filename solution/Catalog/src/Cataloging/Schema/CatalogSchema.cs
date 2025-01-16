using Cataloging.Schema.Types;

namespace Cataloging.Schema;

public class CatalogSchema : GraphQL.Types.Schema
{
    public CatalogSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<CatalogQuery>();
    }
}