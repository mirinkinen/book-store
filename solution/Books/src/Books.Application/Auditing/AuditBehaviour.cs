using MediatR;

namespace Books.Application.Auditing;

internal class AuditBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IAuditContext _auditContext;

    public AuditBehaviour(IAuditContext auditContext)
    {
        _auditContext = auditContext;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IAuditRequest<TResponse> auditRequest)
        {
            return next();
        }

        try
        {
            _auditContext.ActorId = auditRequest.Actor.Id;
            _auditContext.OperationType = auditRequest.OperationType;
            _auditContext.Timestamp = DateTime.UtcNow;

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