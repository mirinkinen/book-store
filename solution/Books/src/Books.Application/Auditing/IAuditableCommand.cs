using MediatR;

namespace Books.Application.Auditing;

internal interface IAuditableCommand<TResponse> : IRequest<TResponse>
{
    User Actor { get; }

    OperationType OperationType { get; }

    Guid ResourceId { get; }

    ResourceType ResourceType { get; }
}