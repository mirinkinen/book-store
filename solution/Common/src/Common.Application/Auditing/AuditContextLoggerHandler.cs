namespace Common.Application.Auditing;

public static class AuditContextLoggerHandler
{
    public static Task Handle(AuditContext auditContext, IAuditContextPublisher publisher)
    {
        ArgumentNullException.ThrowIfNull(publisher);

        return publisher.PublishAuditContext(auditContext);
    }
}