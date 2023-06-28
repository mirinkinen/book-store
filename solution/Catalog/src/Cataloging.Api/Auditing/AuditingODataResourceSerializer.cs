using Cataloging.Application.Auditing;
using Cataloging.Domain.SeedWork;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace Cataloging.Api.Auditing;

public class AuditingODataResourceSerializer : ODataResourceSerializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditingODataResourceSerializer(IODataSerializerProvider serializerProvider, IHttpContextAccessor httpContextAccessor)
        : base(serializerProvider)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task WriteObjectInlineAsync(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
    {
        if (graph is IEdmStructuredObject structuredObject)
        {
            if (structuredObject.TryGetPropertyValue(nameof(Entity.Id), out var id) && id is Guid guid)
            {
                LogId(expectedType, guid);
            }
        }
        else if (graph is Entity entity)
        {
            LogId(expectedType, entity.Id);
        }

        return base.WriteObjectInlineAsync(graph, expectedType, writer, writeContext);
    }

    private void LogId(IEdmTypeReference expectedType, Guid id)
    {
        var auditContext = _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IAuditContext>();
        var type = expectedType?.Definition.ToString();

        if (auditContext != null && type != null)
        {
            var typeName = type.Split('.').Last();
            if (typeName != null && Enum.TryParse(typeName, true, out ResourceType resourceType) && Enum.IsDefined(resourceType))
            {
                auditContext.AddResource(resourceType, id);
                return;
            }

            throw new ArgumentException($"Entity type {typeName} cannot be mapped into ResourceType.");
        }
    }
}