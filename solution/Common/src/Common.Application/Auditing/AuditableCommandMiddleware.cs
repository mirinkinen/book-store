using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Common.Application.Auditing;

/// <summary>
/// Middleware for AuditableCommands. Executed by Wolverine.
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Envelope is never null.")]
public static class AuditableCommandMiddleware
{
    public static void Before(Envelope envelope, IAuditContext auditContext)
    {
        if (envelope.Message is IAuditableCommand auditableCommand)
        {
            auditContext.ActorId = auditableCommand.Actor.Id;
            auditContext.AddResource(auditableCommand.ResourceType, auditableCommand.ResourceId);
            auditContext.OperationType = auditableCommand.OperationType;
            auditContext.Timestamp = DateTime.UtcNow;
        }
    }

    public static void After(IAuditContext auditContext)
    {
        auditContext.Success = true;
    }

    public static ValueTask Finally(IAuditContext auditContext, IMessageBus bus)
    {
        auditContext.Success = true;

        return bus.SendAsync(auditContext);
    }
}