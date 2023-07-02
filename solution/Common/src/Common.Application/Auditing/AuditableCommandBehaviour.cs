using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Common.Application.Auditing;

public class AuditableCommandBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IAuditContext _auditContext;

    public AuditableCommandBehaviour(IAuditContext auditContext)
    {
        _auditContext = auditContext;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "MediatR guarantees non-null delegates")]
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IAuditableCommand<TResponse> auditableCommand)
        {
            return next();
        }

        try
        {
            _auditContext.ActorId = auditableCommand.Actor.Id;
            _auditContext.OperationType = auditableCommand.OperationType;
            _auditContext.Timestamp = DateTime.UtcNow;
            _auditContext.AddResource(auditableCommand.ResourceType, auditableCommand.ResourceId);

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