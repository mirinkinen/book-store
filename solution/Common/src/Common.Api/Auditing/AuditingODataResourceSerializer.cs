using Common.Application.Auditing;
using Common.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace Common.Api.Auditing;

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
        var auditOptions = _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IOptions<AuditOptions>>();
        
        if (auditOptions.Value.Enabled)
        {
            if (graph is IEdmStructuredObject structuredObject)
            {
                if (structuredObject.TryGetPropertyValue(nameof(IIdentifiable.Id), out var id) && id is Guid guid)
                {
                    AuditLogResourceId(expectedType, guid);
                }
            }
            else if (graph is IIdentifiable identifiable)
            {
                AuditLogResourceId(expectedType, identifiable.Id);
            }
        }

        return base.WriteObjectInlineAsync(graph, expectedType, writer, writeContext);
    }

    private void AuditLogResourceId(IEdmTypeReference expectedType, Guid id)
    {
        var auditContext = _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<AuditContext>();

        if (auditContext == null)
        {
            throw new InvalidOperationException("AuditContext not found as required service.");
        }

        var type = expectedType?.Definition?.ToString()?.Split('.').Last();
        if (type == null)
        {
            throw new ArgumentException($"Entity type {type} cannot be mapped into ResourceType.");
        }

        auditContext.AddResource(id, type);
    }
}