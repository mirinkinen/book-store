namespace Common.Application.Auditing;

public interface IAuditFeature
{
    public IAuditContext AuditContext { get; }
}