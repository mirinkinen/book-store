using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Shared.Application.Auditing;

[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Envelope is never null.")]
public static class AuditableCommandMiddleware
{
    public static void Before(Envelope envelope, IAuditContext auditContext)
    {
        if (envelope.Message is IAuditableCommand auditableCommand)
        {
            auditContext.Timestamp = DateTime.UtcNow;
            auditContext.ActorId = auditableCommand.Actor.Id;
            auditContext.OperationType = auditableCommand.OperationType;
            auditContext.AddResource(auditableCommand.ResourceType, auditableCommand.ResourceId);
        }
    }

    public static void After(IAuditContext auditContext)
    {
        auditContext.Success = true;
    }
}