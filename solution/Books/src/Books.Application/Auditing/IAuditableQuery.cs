using MediatR;

namespace Books.Application.Auditing;

internal interface IAuditableQuery<TResponse> : IRequest<TResponse>
{
    User Actor { get; }

    OperationType OperationType => OperationType.Read;
}