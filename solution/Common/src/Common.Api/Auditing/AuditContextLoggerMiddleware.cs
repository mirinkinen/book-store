using Common.Application.Auditing;
using Common.Application.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace Common.Api.Auditing;

public class AuditContextLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMessageBus _bus;
    private readonly IUserService _userService;

    /// <summary>
    /// Publishes audit context after handlers. Should be only used for queries.
    /// </summary>
    public AuditContextLoggerMiddleware(RequestDelegate next, IMessageBus bus, IUserService userService)
    {
        _next = next;
        _bus = bus;
        _userService = userService;
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

        var auditLogEvent = new AuditLogEvent(_userService.GetUser().Id, OperationType.Read, auditContext.Resources);
        await _bus.SendAsync(auditLogEvent);
    }
}