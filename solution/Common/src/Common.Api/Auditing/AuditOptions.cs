namespace Common.Api.Auditing;

public class AuditOptions
{
    public const string Audit = "Audit";

    public bool Enabled { get; set; } = true;
}