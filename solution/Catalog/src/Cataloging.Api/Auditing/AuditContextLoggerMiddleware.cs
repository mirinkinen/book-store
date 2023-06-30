using Microsoft.Extensions.Options;
using Shared.Application.Auditing;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Api.Auditing;

public class AuditContextLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<AuditOptions> _auditOptions;
    private readonly ILogger<AuditContextLoggerMiddleware> _logger;
    private static readonly string _separator = new('-', 30);

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

        _logger.LogDebug(string.Empty);
        _logger.LogDebug(_separator);
        _logger.LogDebug("Success: {Success}", auditContext.Success);
        _logger.LogDebug("User: {ActorId}", auditContext.ActorId);
        _logger.LogDebug("Operation: {OperationType}", auditContext.OperationType);

        foreach (var resource in auditContext.Resources)
        {
            _logger.LogDebug("Type: {Type}: Id: {Id}", resource.Type, resource.Id);
        }

        _logger.LogDebug(_separator);
        _logger.LogDebug(string.Empty);
    }
}