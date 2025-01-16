namespace Common.Api.Application.Auditing;

public record AuditLogEvent(Guid ActorId, OperationType OperationType, IEnumerable<AuditLogResource> Resources);