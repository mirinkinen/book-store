using Microsoft.Extensions.Options;
using Shared.Application.Auditing;
using System.Diagnostics;

namespace Cataloging.Api.Auditing;

public class AuditContextLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<AuditOptions> _auditOptions;

    public AuditContextLoggerMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, IOptions<AuditOptions> auditOptions)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
        _auditOptions = auditOptions;
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

        Debug.WriteLine(string.Empty);
        Debug.WriteLine(new string('-', 30));

        Debug.WriteLine($"Success: {auditContext.Success}");
        Debug.WriteLine($"User: {auditContext.ActorId}");
        Debug.WriteLine($"Operation: {auditContext.OperationType}");
        foreach (var resource in auditContext.Resources)
        {
            Debug.WriteLine($"{resource.Type}: {resource.Id}");
        }

        Debug.WriteLine(new string('-', 30));
        Debug.WriteLine(string.Empty);
    }
}