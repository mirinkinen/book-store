using Microsoft.Extensions.Logging;

namespace Common.Application.Auditing;

/// <summary>
/// Middleware for AuditableCommands. Executed by Wolverine.
/// </summary>
public class AuditLogEventHandler
{
    private static readonly Action<ILogger, Guid, OperationType, Exception?> _logAudit =
        LoggerMessage.Define<Guid, OperationType>(LogLevel.Information, new EventId(0),
            "Audit logging, User: {UserId}, OperationType: {OperationType}");

    private static readonly Action<ILogger, string, Guid, Exception?> _logResource =
        LoggerMessage.Define<string, Guid>(LogLevel.Information, new EventId(0),
            "Type: {Type}: Id: {Id}");

    public void Handle(AuditLogEvent auditLogEvent, ILogger<AuditLogEventHandler> logger)
    {
        _logAudit.Invoke(logger, auditLogEvent.ActorId, auditLogEvent.OperationType, null);

        foreach (var resource in auditLogEvent.Resources)
        {
            _logResource.Invoke(logger, resource.ResourceType, resource.ResourceId, null);
        }
    }
}