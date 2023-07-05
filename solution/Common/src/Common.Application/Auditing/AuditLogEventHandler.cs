using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Common.Application.Auditing;

/// <summary>
/// Middleware for AuditableCommands. Executed by Wolverine.
/// </summary>
[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix")]
public class AuditLogEventHandler
{
    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public void Handle(AuditLogEvent auditLogEvent, ILogger<AuditLogEventHandler> logger)
    {
        // This could persist the events, but now we are just logging.
        using var scope = logger.BeginScope("Audit logging");
        logger.LogInformation("User: {ActorId}", auditLogEvent.ActorId);
        logger.LogInformation("Operation: {OperationType}", auditLogEvent.OperationType);

        foreach (var resource in auditLogEvent.Resources)
        {
            logger.LogInformation("Type: {Type}: Id: {Id}", resource.ResourceType, resource.ResourceId);
        }
    }
}