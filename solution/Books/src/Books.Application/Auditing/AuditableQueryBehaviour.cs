using MediatR;

namespace Books.Application.Auditing;

internal class AuditableQueryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IAuditContext _auditContext;

    public AuditableQueryBehaviour(IAuditContext auditContext)
    {
        _auditContext = auditContext;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IAuditableQuery<TResponse> auditableQuery)
        {
            return next();
        }

        try
        {
            _auditContext.ActorId = auditableQuery.Actor.Id;
            _auditContext.OperationType = auditableQuery.OperationType;
            _auditContext.Timestamp = DateTime.UtcNow;
            // Queried resources are not known at this point. They are set later when OData query is serialized.

            var response = next();

            _auditContext.Success = true;

            return response;
        }
        catch
        {
            _auditContext.Success = false;
            throw;
        }
    }
}