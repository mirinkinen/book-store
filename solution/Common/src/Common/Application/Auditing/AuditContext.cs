using System.Collections.ObjectModel;

namespace Common.Api.Application.Auditing;

public class AuditContext
{
    private readonly List<AuditLogResource> _resources = new();

    public ReadOnlyCollection<AuditLogResource> Resources => _resources.AsReadOnly();

    public void AddResource(Guid resourceId, string resourceType)
    {
        _resources.Add(new AuditLogResource(resourceId, resourceType));
    }
}