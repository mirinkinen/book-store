using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;

namespace Books.Api.OData.Serialization;

public class CustomODataEntityReferenceLinksSerializer : ODataEntityReferenceLinksSerializer
{
    public CustomODataEntityReferenceLinksSerializer()
    {
    }

    public override Task WriteObjectAsync(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
    {
        return base.WriteObjectAsync(graph, type, messageWriter, writeContext);
    }
}