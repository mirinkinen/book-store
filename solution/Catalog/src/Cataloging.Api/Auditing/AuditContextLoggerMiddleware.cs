using Microsoft.Extensions.Options;
using Shared.Application.Auditing;

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

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (!_auditOptions.Value.Enabled)
        {
            return;
        }

        var auditContext = _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IAuditContext>();
        if (auditContext == null)
        {
            return;
        }

        _logger.LogDebug(string.Empty);
        _logger.LogDebug(new string('-', 30));
        _logger.LogDebug($"Success: {auditContext.Success}");
        _logger.LogDebug($"User: {auditContext.ActorId}");
        _logger.LogDebug($"Operation: {auditContext.OperationType}");

        foreach (var resource in auditContext.Resources)
        {
            _logger.LogDebug($"{resource.Type}: {resource.Id}");
        }

        _logger.LogDebug(new string('-', 30));
        _logger.LogDebug(string.Empty);
    }
}