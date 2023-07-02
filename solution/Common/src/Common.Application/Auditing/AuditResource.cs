namespace Common.Application.Auditing;

public record AuditResource(ResourceType Type, Guid Id);