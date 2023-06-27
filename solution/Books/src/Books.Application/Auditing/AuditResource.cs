namespace Books.Application.Auditing;

public record AuditResource(ResourceType Type, Guid Id);