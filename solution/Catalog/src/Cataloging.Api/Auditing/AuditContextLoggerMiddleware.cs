using Microsoft.Extensions.Options;
using Common.Application.Auditing;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Api.Auditing;

public class AuditContextLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<AuditOptions> _auditOptions;
    private readonly ILogger<AuditContextLoggerMiddleware> _logger;

    public AuditContextLoggerMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, IOptions<AuditOptions> auditOptions,
        ILogger<AuditContextLoggerMiddleware> logger)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
        _auditOptions = auditOptions;
        _logger = logger;
    }

    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "This is a mock code.")]
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (!_auditOptions.Value.Enabled)
        {
            return;
        }

        var auditContext = _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IAuditContext>();

        using var scope = _logger.BeginScope("Audit logging");
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _logger.LogDebug("Success: {Success}", auditContext.Success);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        _logger.LogDebug("User: {ActorId}", auditContext.ActorId);
        _logger.LogDebug("Operation: {OperationType}", auditContext.OperationType);

        var resourcesScope = _logger.BeginScope("Audit resources");
        foreach (var resource in auditContext.Resources)
        {
            _logger.LogDebug("Type: {Type}: Id: {Id}", resource.Type, resource.Id);
        }
        resourcesScope?.Dispose();

    }
}