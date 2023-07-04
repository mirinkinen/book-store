using System.Diagnostics.CodeAnalysis;
using Wolverine;

namespace Common.Application.Auditing;

[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
public static class AuditableQueryMiddleware
{
    public static void Before(Envelope envelope)
    {
        if (envelope.Message is not IAuditableQuery auditableQuery)
        {
            return;
        }

        var auditContext = auditableQuery.AuditContext;

        auditContext.ActorId = auditableQuery.Actor.Id;
        auditContext.OperationType = auditableQuery.OperationType;
        auditContext.Timestamp = DateTime.UtcNow;
    }

    public static void After(Envelope envelope)
    {
        if (envelope.Message is not IAuditableQuery auditableQuery)
        {
            return;
        }

        auditableQuery.AuditContext.Success = true;
    }

    public static ValueTask Finally(Envelope envelope)
    {
        if (envelope.Message is not IAuditableQuery auditableQuery)
        {
            return ValueTask.CompletedTask;
        }

        var auditContext = auditableQuery.AuditContext;

        auditContext.Success = true;

        // Don't send the message here since the audit context is not yet complete.
        // The AuditContextLoggerMiddleware will sent the message.
        return ValueTask.CompletedTask;
    }
}