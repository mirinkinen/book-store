using System.Collections.ObjectModel;

namespace Books.Application.Auditing;

public class AuditContext : IAuditContext
{
    public Guid ActorId { get; set; }

    public OperationType OperationType { get; set; }

    private readonly List<AuditResource> _resources = new();
    public ReadOnlyCollection<AuditResource> Resources => _resources.AsReadOnly();

    public bool Success { get; set; }

    public DateTime Timestamp { get; set; }

    public void AddResource(ResourceType type, Guid id)
    {
        _resources.Add(new AuditResource(type, id));
    }
}