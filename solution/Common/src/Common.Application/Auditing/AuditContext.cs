using System.Collections.ObjectModel;

namespace Common.Application.Auditing;

public class AuditContext : IAuditContext
{
    private readonly List<AuditResource> _resources = new();

    public Guid ActorId { get; set; }

    public OperationType OperationType { get; set; }

    public ReadOnlyCollection<AuditResource> Resources => _resources.AsReadOnly();

    public int StatusCode { get; set; }

    public bool Success { get; set; }

    public DateTime Timestamp { get; set; }

    public void AddResource(ResourceType resourceType, Guid resourceId)
    {
        _resources.Add(new AuditResource(resourceType, resourceId));
    }
}