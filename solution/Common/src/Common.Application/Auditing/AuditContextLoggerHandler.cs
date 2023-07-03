using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Common.Application.Auditing;

public class AuditContextLoggerHandler
{
    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
    public static void Handle(AuditContext auditContext, ILogger<AuditContextLoggerHandler> logger)
    {
        logger.LogInformation("Persisting audit context");

        logger.LogInformation("Success: {Success}", auditContext.Success);
        logger.LogInformation("User: {ActorId}", auditContext.ActorId);
        logger.LogInformation("Operation: {OperationType}", auditContext.OperationType);

        foreach (var resource in auditContext.Resources)
        {
            logger.LogInformation("Type: {Type}: Id: {Id}", resource.Type, resource.Id);
        }
    }
}