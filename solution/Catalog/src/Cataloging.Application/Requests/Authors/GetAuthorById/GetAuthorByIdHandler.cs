using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Common.Application;
using Common.Application.Auditing;
using Common.Application.Authentication;

namespace Cataloging.Application.Requests.Authors.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId, User Actor, IQueryAuthorizer QueryAuthorizer, IAuditContext AuditContext)
    : IAuditableQuery;

public static class GetAuthorByIdHandler
{
    public static QueryableResponse<Author> Handle(GetAuthorByIdQuery request)
    {
        return new QueryableResponse<Author>(
            request.QueryAuthorizer.GetAuthorizedEntities<Author>()
                .Where(a => a.Id == request.AuthorId));
    }
}