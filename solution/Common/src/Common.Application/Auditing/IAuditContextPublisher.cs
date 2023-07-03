namespace Common.Application.Auditing;

public interface IAuditContextPublisher
{
    Task PublishAuditContext(IAuditContext auditContext);
}