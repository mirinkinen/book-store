using System.Collections.ObjectModel;

namespace Books.Application.Auditing;

public interface IAuditContext
{
    Guid ActorId { get; set; }

    OperationType OperationType { get; set; }

    ReadOnlyCollection<AuditResource> Resources { get; }

    bool Success { get; set; }

    DateTime Timestamp { get; set; }

    void AddResource(ResourceType resourceType, Guid resourceId);
}