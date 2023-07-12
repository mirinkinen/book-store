using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace Common.Api.Auditing;

public class AuditContextLoggerMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Publishes audit context after handlers. Should be only used for queries.
    /// </summary>
    public AuditContextLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        await _next(context);

        var auditContext = context.RequestServices.GetRequiredService<AuditContext>();
        if (!auditContext.Resources.Any())
        {
            return;
        }

        var userService = context.RequestServices.GetRequiredService<IUserService>();
        var bus = context.RequestServices.GetRequiredService<IMessageBus>();
        
        var auditLogEvent = new AuditLogEvent(userService.GetUser().Id, OperationType.Read, auditContext.Resources);
        await bus.SendAsync(auditLogEvent);
    }
}