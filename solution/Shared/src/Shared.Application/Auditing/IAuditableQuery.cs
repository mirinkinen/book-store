using Shared.Application.Authentication;

namespace Shared.Application.Auditing;

public interface IAuditableQuery
{
    User Actor { get; }

    OperationType OperationType => OperationType.Read;
}