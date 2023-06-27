using MediatR;

namespace Books.Application.Auditing;

internal interface IAuditRequest<TResponse> : IRequest<TResponse>
{
    OperationType OperationType { get; }

    User Actor { get; }
}