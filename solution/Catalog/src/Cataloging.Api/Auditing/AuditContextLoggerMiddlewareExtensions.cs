namespace Cataloging.Api.Auditing;

public static class AuditContextLoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseAuditLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuditContextLoggerMiddleware>();
    }
}