using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;

namespace Books.Api.OData.Serialization;

public class CustomODataEntityReferenceLinkSerializer : ODataEntityReferenceLinkSerializer
{
    public CustomODataEntityReferenceLinkSerializer()
    {
    }

    public override Task WriteObjectAsync(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
    {
        return base.WriteObjectAsync(graph, type, messageWriter, writeContext);
    }
}
