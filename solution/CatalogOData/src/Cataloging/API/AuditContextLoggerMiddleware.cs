using Cataloging.Application.Auditing;
using Common.Application.Authentication;
using Wolverine;

namespace Cataloging.API;

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
        if (auditContext.Resources.Count == 0)
        {
            return;
        }

        var userService = context.RequestServices.GetRequiredService<IUserAccessor>();
        var user = await userService.GetUser();

        var auditLogEvent = new AuditLogEvent(user.Id, OperationType.Read, auditContext.Resources);

        var bus = context.RequestServices.GetRequiredService<IMessageBus>();
        await bus.SendAsync(auditLogEvent);
    }
}