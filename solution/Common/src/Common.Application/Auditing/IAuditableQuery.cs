using Common.Application.Authentication;

namespace Common.Application.Auditing;

public interface IAuditableQuery
{
    User Actor { get; }

    IAuditContext AuditContext { get; }

    OperationType OperationType => OperationType.Read;
}