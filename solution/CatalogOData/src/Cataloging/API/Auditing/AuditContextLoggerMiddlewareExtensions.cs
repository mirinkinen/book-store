// Namespace must be "Microsoft.AspNetCore.Builder" for easier usage.

namespace Cataloging.API.Auditing;

public static class AuditContextLoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditContextLoggerMiddleware>();
    }
}