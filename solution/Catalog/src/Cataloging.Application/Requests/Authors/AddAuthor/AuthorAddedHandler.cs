using Common.Application.Auditing;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Application.Requests.Authors.AddAuthor;

public static class AuthorAddedHandler
{
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
    public static Task Handle(AuthorAdded authorAdded, IAuditContextPublisher auditContextPublisher)
    {
        var auditContext = new AuditContext
        {
            ActorId = authorAdded.ActorId,
            OperationType = OperationType.Create,
            Success = true,
            Timestamp = DateTime.UtcNow
        };

        auditContext.AddResource(ResourceType.Author, authorAdded.AuthorId);

        return auditContextPublisher.PublishAuditContext(auditContext);
    }
}