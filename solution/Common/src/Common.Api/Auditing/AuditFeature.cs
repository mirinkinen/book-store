using Common.Application.Auditing;

namespace Common.Api.Auditing;

public class AuditFeature : IAuditFeature
{
    public IAuditContext AuditContext { get; }

    public AuditFeature(IAuditContext auditContext)
    {
        AuditContext = auditContext ?? throw new ArgumentNullException(nameof(auditContext));
    }
}