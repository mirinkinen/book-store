using MediatR;

namespace Cataloging.Application.Auditing;

internal interface IAuditableQuery<TResponse> : IRequest<TResponse>
{
    User Actor { get; }

    OperationType OperationType => OperationType.Read;
}