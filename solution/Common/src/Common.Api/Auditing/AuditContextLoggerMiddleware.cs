using Common.Application.Auditing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Api.Auditing;

public class AuditContextLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuditContextPublisher _publisher;

    /// <summary>
    /// Publishes audit context after handlers. Should be only used for queries.
    /// </summary>
    public AuditContextLoggerMiddleware(RequestDelegate next, IAuditContextPublisher publisher)
    {
        _next = next;
        _publisher = publisher;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        await _next(context);

        var auditContext = context.RequestServices.GetRequiredService<IAuditContext>();
        if (auditContext.ActorId != Guid.Empty)
        {
            await _publisher.PublishAuditContext(auditContext);
        }
    }
}