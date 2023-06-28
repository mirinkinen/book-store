using System.Collections.ObjectModel;

namespace Shared.Application.Auditing;

public class AuditContext : IAuditContext
{
    public Guid ActorId { get; set; }

    public OperationType OperationType { get; set; }

    private readonly List<AuditResource> _resources = new();
    public ReadOnlyCollection<AuditResource> Resources => _resources.AsReadOnly();

    public bool Success { get; set; }

    public DateTime Timestamp { get; set; }

    public void AddResource(ResourceType resourceType, Guid resourceId)
    {
        _resources.Add(new AuditResource(resourceType, resourceId));
    }
}