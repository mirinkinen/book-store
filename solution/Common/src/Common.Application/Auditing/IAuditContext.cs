using System.Collections.ObjectModel;

namespace Common.Application.Auditing;

public interface IAuditContext
{
    Guid ActorId { get; set; }

    OperationType OperationType { get; set; }

    ReadOnlyCollection<AuditResource> Resources { get; }

    int StatusCode { get; set; }

    bool Success { get; set; }

    DateTime Timestamp { get; set; }

    void AddResource(ResourceType resourceType, Guid resourceId);
}