using MediatR;
using Common.Application.Authentication;

namespace Common.Application.Auditing;

public interface IAuditableQuery
{
    User Actor { get; }

    OperationType OperationType => OperationType.Read;
}

// Only for MediatR.
public interface IAuditableQuery<TResponse> : IAuditableQuery, IRequest<TResponse>
{
}