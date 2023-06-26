using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System.Collections;

namespace Books.Api.OData.Serialization;

public class CustomODataCollectionSerializer : ODataCollectionSerializer
{
    public CustomODataCollectionSerializer(IODataSerializerProvider serializerProvider) : base(serializerProvider)
    {
    }

    public override ODataCollectionValue CreateODataCollectionValue(IEnumerable enumerable, IEdmTypeReference elementType, ODataSerializerContext writeContext)
    {
        return base.CreateODataCollectionValue(enumerable, elementType, writeContext);
    }

    public override Task WriteCollectionAsync(ODataCollectionWriter writer, object graph, IEdmTypeReference collectionType, ODataSerializerContext writeContext)
    {
        return base.WriteCollectionAsync(writer, graph, collectionType, writeContext);
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