using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Common.Application.Auditing;

public class AuditContextPublisher : IAuditContextPublisher
{
    private readonly ILogger<AuditContextPublisher> _logger;

    public AuditContextPublisher(ILogger<AuditContextPublisher> logger)
    {
        _logger = logger;
    }

    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
    public Task PublishAuditContext(IAuditContext auditContext)
    {
        ArgumentNullException.ThrowIfNull(auditContext);

        using var scope = _logger.BeginScope("Audit logging");
        _logger.LogInformation("Success: {Success}", auditContext.Success);
        _logger.LogInformation("User: {ActorId}", auditContext.ActorId);
        _logger.LogInformation("Operation: {OperationType}", auditContext.OperationType);

        foreach (var resource in auditContext.Resources)
        {
            _logger.LogInformation("Type: {Type}: Id: {Id}", resource.Type, resource.Id);
        }

        return Task.CompletedTask;
    }
}