using Common.Application.Authentication;

namespace Common.Application.Auditing;

public interface IAuditableCommand
{
    User Actor { get; }

    OperationType OperationType { get; }

    Guid ResourceId { get; }

    ResourceType ResourceType { get; }
}