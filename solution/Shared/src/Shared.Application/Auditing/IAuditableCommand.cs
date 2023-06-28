using MediatR;
using Shared.Application.Authentication;

namespace Shared.Application.Auditing;

public interface IAuditableCommand<TResponse> : IRequest<TResponse>
{
    User Actor { get; }

    OperationType OperationType { get; }

    Guid ResourceId { get; }

    ResourceType ResourceType { get; }
}