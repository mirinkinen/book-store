using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Common.Application;
using Common.Application.Auditing;
using Common.Application.Authentication;

namespace Cataloging.Application.Requests.Authors.GetAuthors;

public record GetAuthorsQuery(User Actor, IQueryAuthorizer QueryAuthorizer, IAuditContext AuditContext) : IAuditableQuery;

public static class GetAuthorsHandler
{
    public static QueryableResponse<Author> Handle(GetAuthorsQuery request)
    {
        return new QueryableResponse<Author>(
            request.QueryAuthorizer.GetAuthorizedEntities<Author>());
    }
}