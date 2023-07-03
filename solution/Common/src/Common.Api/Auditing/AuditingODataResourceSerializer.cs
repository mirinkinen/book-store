using Common.Application.Auditing;
using Common.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter.Value;
using Microsoft.Extensions.Options;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace Common.Api.Auditing;

public class AuditingODataResourceSerializer : ODataResourceSerializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<AuditOptions> _auditOptions;

    public AuditingODataResourceSerializer(IODataSerializerProvider serializerProvider, IHttpContextAccessor httpContextAccessor,
        IOptions<AuditOptions> auditOptions)
        : base(serializerProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _auditOptions = auditOptions;
    }

    public override Task WriteObjectInlineAsync(object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)
    {
        if (_auditOptions.Value.Enabled)
        {
            if (graph is IEdmStructuredObject structuredObject)
            {
                if (structuredObject.TryGetPropertyValue(nameof(IIdentifiable.Id), out var id) && id is Guid guid)
                {
                    LogId(expectedType, guid);
                }
            }
            else if (graph is IIdentifiable identifiable)
            {
                LogId(expectedType, identifiable.Id);
            }
        }

        return base.WriteObjectInlineAsync(graph, expectedType, writer, writeContext);
    }

    private void LogId(IEdmTypeReference expectedType, Guid id)
    {
        var auditContext = _httpContextAccessor.HttpContext?.Features.Get<IAuditFeature>().AuditContext;
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