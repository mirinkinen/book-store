using Common.Application.Auditing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace Common.Api.Auditing;

public class AuditContextLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMessageBus _bus;

    /// <summary>
    /// Publishes audit context after handlers. Should be only used for queries.
    /// </summary>
    public AuditContextLoggerMiddleware(RequestDelegate next, IMessageBus bus)
    {
        _next = next;
        _bus = bus;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        await _next(context);

        var auditContext = context.RequestServices.GetRequiredService<IAuditContext>();
        if (auditContext.ActorId != Guid.Empty)
        {
            await _bus.SendAsync(auditContext);
        }
    }
}