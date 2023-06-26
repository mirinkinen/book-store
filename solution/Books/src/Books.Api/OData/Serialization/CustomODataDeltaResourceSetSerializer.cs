using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System.Collections;

namespace Books.Api.OData.Serialization;

public class CustomODataDeltaResourceSetSerializer : ODataDeltaResourceSetSerializer
{
    public CustomODataDeltaResourceSetSerializer(IODataSerializerProvider serializerProvider) : base(serializerProvider)
    {
    }

    public override Task WriteDeltaDeletedLinkAsync(object value, ODataWriter writer, ODataSerializerContext writeContext)
    {
        return base.WriteDeltaDeletedLinkAsync(value, writer, writeContext);
    }

    public override ODataDeltaResourceSet CreateODataDeltaResourceSet(IEnumerable feedInstance, IEdmCollectionTypeReference feedType, ODataSerializerContext writeContext)
    {
        return base.CreateODataDeltaResourceSet(feedInstance, feedType, writeContext);
    }

    public override ODataValue CreateODataValue(object graph, IEdmTypeReference expectedType, ODataSerializerContext writeContext)
    {
        return base.CreateODataValue(graph, expectedType, writeContext);
    }

    public override Task WriteDeltaDeletedResourceAsync(object value, ODataWriter writer, ODataSerializerContext writeContext)
    {
        return base.WriteDeltaDeletedResourceAsync(value, writer, writeContext);
    }

    public override Task WriteDeltaLinkAsync(object value, ODataWriter writer, ODataSerializerContext writeContext)
    {
        return base.WriteDeltaLinkAsync(value, writer, writeContext);
    }

    public override Task WriteObjectAsync(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
    {
        return base.WriteObjectAsync(graph, type, messageWriter, writeContext);
    }

    public override Task WriteObjectInlineAsync(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
    {
        return base.WriteObjectInlineAsync(graph, expectedType, writer, writeContext);
    }
}