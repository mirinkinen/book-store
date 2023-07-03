using Common.Application.Authentication;

namespace Common.Application.Auditing;

public record AuditedEvent(User Actor, OperationType OperationType, ResourceType ResourceType, Guid ResourceId);