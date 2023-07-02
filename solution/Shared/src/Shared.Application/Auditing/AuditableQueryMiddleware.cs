using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Shared.Application.Auditing;

[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Envelope is never null.")]
public static class AuditableQueryMiddleware
{
    public static void Before(Envelope envelope, IAuditContext auditContext)
    {
        if (envelope.Message is IAuditableQuery auditableQuery)
        {
            auditContext.Timestamp = DateTime.UtcNow;
            auditContext.ActorId = auditableQuery.Actor.Id;
            auditContext.OperationType = auditableQuery.OperationType;
        }
    }

    public static void After(IAuditContext auditContext)
    {
        auditContext.Success = true;
    }
}