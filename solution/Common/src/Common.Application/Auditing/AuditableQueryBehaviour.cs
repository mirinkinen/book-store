using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Common.Application.Auditing;

public class AuditableQueryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditableQueryBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "MediatR guarantees non-null delegates")]
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IAuditableQuery<TResponse> auditableQuery)
        {
            return next();
        }

        var auditContext = _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IAuditContext>();

        try
        {
            auditContext.ActorId = auditableQuery.Actor.Id;
            auditContext.OperationType = auditableQuery.OperationType;
            auditContext.Timestamp = DateTime.UtcNow;
            // Queried resources are not known at this point. They are set later when OData query is serialized.

            var response = next();

            auditContext.Success = true;

            return response;
        }
        catch
        {
            auditContext.Success = false;
            throw;
        }
        finally
        {
            auditContext.StatusCode = _httpContextAccessor.HttpContext.Response.StatusCode;
        }
    }
}