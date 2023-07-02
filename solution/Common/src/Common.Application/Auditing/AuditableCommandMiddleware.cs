using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Common.Application.Auditing;

/// <summary>
/// Middleware for AuditableCommands. Executed by Wolverine.
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Envelope is never null.")]
public class AuditableCommandMiddleware
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditableCommandMiddleware(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Before(Envelope envelope, IAuditContext auditContext)
    {
        if (envelope.Message is IAuditableCommand auditableCommand)
        {
            auditContext.ActorId = auditableCommand.Actor.Id;
            auditContext.AddResource(auditableCommand.ResourceType, auditableCommand.ResourceId);
            auditContext.OperationType = auditableCommand.OperationType;
            auditContext.Timestamp = DateTime.UtcNow;
        }
    }

    public void After(IAuditContext auditContext)
    {
        auditContext.Success = true;
    }

    public void Finally(IAuditContext auditContext)
    {
        auditContext.StatusCode = _httpContextAccessor.HttpContext.Response.StatusCode;
        auditContext.Success = true;
    }
}