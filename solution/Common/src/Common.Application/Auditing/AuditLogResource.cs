namespace Common.Application.Auditing;

public record AuditLogResource(Guid ResourceId, string ResourceType);