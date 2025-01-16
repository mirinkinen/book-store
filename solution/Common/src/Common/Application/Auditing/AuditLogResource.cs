namespace Common.Api.Application.Auditing;

public record AuditLogResource(Guid ResourceId, string ResourceType);