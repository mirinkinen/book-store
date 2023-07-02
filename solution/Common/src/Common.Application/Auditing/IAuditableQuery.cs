using MediatR;
using Common.Application.Authentication;

namespace Common.Application.Auditing;

public interface IAuditableQuery
{
    User Actor { get; }

    OperationType OperationType => OperationType.Read;
}

public interface IAuditableQuery<TResponse> : IRequest<TResponse>
{
    User Actor { get; }

    OperationType OperationType => OperationType.Read;
}