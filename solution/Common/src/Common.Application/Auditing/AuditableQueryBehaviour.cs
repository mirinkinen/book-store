using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace Common.Application.Auditing;

public class AuditableQueryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IAuditContext _auditContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditableQueryBehaviour(IAuditContext auditContext, IHttpContextAccessor httpContextAccessor)
    {
        _auditContext = auditContext;
        _httpContextAccessor = httpContextAccessor;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "MediatR guarantees non-null delegates")]
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
        finally
        {
            _auditContext.StatusCode = _httpContextAccessor.HttpContext.Response.StatusCode;
        }
    }
}