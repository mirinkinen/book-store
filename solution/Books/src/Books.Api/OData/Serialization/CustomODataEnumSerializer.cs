using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace Books.Api.OData.Serialization;

public class CustomODataEnumSerializer : ODataEnumSerializer
{
    public CustomODataEnumSerializer(IODataSerializerProvider serializerProvider) : base(serializerProvider)
    {
    }

    public override ODataEnumValue CreateODataEnumValue(object graph, IEdmEnumTypeReference enumType, ODataSerializerContext writeContext)
    {
        return base.CreateODataEnumValue(graph, enumType, writeContext);
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