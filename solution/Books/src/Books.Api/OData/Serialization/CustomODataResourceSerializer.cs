using Books.Application.Services;
using Books.Domain.SeedWork;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Books.Api.OData.Serialization;

public class CustomODataResourceSerializer : ODataResourceSerializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomODataResourceSerializer(IODataSerializerProvider serializerProvider, IHttpContextAccessor httpContextAccessor) : base(serializerProvider)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public override void AppendDynamicProperties(ODataResource resource, SelectExpandNode selectExpandNode, ResourceContext resourceContext)
    {
        base.AppendDynamicProperties(resource, selectExpandNode, resourceContext);
    }

    public override ODataNestedResourceInfo CreateComplexNestedResourceInfo(IEdmStructuralProperty complexProperty, PathSelectItem pathSelectItem, ResourceContext resourceContext)
    {
        return base.CreateComplexNestedResourceInfo(complexProperty, pathSelectItem, resourceContext);
    }

    public override ODataProperty CreateComputedProperty(string propertyName, ResourceContext resourceContext)
    {
        return base.CreateComputedProperty(propertyName, resourceContext);
    }

    public override ODataNestedResourceInfo CreateDynamicComplexNestedResourceInfo(string propertyName, object propertyValue, IEdmTypeReference edmType, ResourceContext resourceContext)
    {
        return base.CreateDynamicComplexNestedResourceInfo(propertyName, propertyValue, edmType, resourceContext);
    }

    public override string CreateETag(ResourceContext resourceContext)
    {
        return base.CreateETag(resourceContext);
    }

    public override ODataNestedResourceInfo CreateNavigationLink(IEdmNavigationProperty navigationProperty, ResourceContext resourceContext)
    {
        return base.CreateNavigationLink(navigationProperty, resourceContext);
    }

    public override ODataAction CreateODataAction(IEdmAction action, ResourceContext resourceContext)
    {
        return base.CreateODataAction(action, resourceContext);
    }

    public override ODataFunction CreateODataFunction(IEdmFunction function, ResourceContext resourceContext)
    {
        return base.CreateODataFunction(function, resourceContext);
    }

    public override ODataValue CreateODataValue(object graph, IEdmTypeReference expectedType, ODataSerializerContext writeContext)
    {
        return base.CreateODataValue(graph, expectedType, writeContext);
    }

    public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
    {
        return base.CreateResource(selectExpandNode, resourceContext);
    }

    public override SelectExpandNode CreateSelectExpandNode(ResourceContext resourceContext)
    {
        return base.CreateSelectExpandNode(resourceContext);
    }

    public override ODataStreamPropertyInfo CreateStreamProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
    {
        return base.CreateStreamProperty(structuralProperty, resourceContext);
    }

    public override ODataProperty CreateStructuralProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
    {
        return base.CreateStructuralProperty(structuralProperty, resourceContext);
    }

    public override ODataNestedResourceInfo CreateUntypedNestedResourceInfo(IEdmStructuralProperty structuralProperty, object propertyValue, IEdmTypeReference valueType, PathSelectItem pathSelectItem, ResourceContext resourceContext)
    {
        return base.CreateUntypedNestedResourceInfo(structuralProperty, propertyValue, valueType, pathSelectItem, resourceContext);
    }

    public override object CreateUntypedPropertyValue(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext, out IEdmTypeReference actualType)
    {
        return base.CreateUntypedPropertyValue(structuralProperty, resourceContext, out actualType);
    }

    public override Task WriteDeltaObjectInlineAsync(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
    {
        return base.WriteDeltaObjectInlineAsync(graph, expectedType, writer, writeContext);
    }

    public override Task WriteObjectAsync(object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)
    {
        return base.WriteObjectAsync(graph, type, messageWriter, writeContext);
    }

    public override Task WriteObjectInlineAsync(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
    {
        if (graph is IEdmStructuredObject structuredObject)
        {
            if (structuredObject.TryGetPropertyValue(nameof(Entity.Id), out var id))
            {
                LogId(expectedType, id);
            }
        }
        else if (graph is Entity entity)
        {
            LogId(expectedType, entity.Id);
        }

        return base.WriteObjectInlineAsync(graph, expectedType, writer, writeContext);
    }

    private void LogId(IEdmTypeReference expectedType, object id)
    {
        var entityAuditor = _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IEntityAuditor>();
        var type = expectedType?.Definition.ToString();

        if (entityAuditor != null && type != null)
        {
            entityAuditor.AddId(type, (Guid)id);
        }
    }
}