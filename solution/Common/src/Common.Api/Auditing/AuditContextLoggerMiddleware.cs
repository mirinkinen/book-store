using Common.Application.Auditing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Common.Api.Auditing;

public class AuditContextLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IOptions<AuditOptions> _auditOptions;
    private readonly ILogger<AuditContextLoggerMiddleware> _logger;

    public AuditContextLoggerMiddleware(RequestDelegate next, IOptions<AuditOptions> auditOptions,
        ILogger<AuditContextLoggerMiddleware> logger)
    {
        _next = next;
        _auditOptions = auditOptions;
        _logger = logger;
    }

    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "This is a mock code.")]
    public async Task InvokeAsync(HttpContext context)
    {
        if (!_auditOptions.Value.Enabled)
        {
            await _next(context);
        }

        // Create audit context that is mutable for the life of the request.
        var auditContext = context.RequestServices.GetRequiredService<IAuditContext>();
        context.Features.Set<IAuditFeature>(new AuditFeature(auditContext));

        await _next(context);

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