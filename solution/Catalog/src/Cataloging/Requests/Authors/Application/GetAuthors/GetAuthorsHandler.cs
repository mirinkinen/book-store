using Cataloging.Application;
using Cataloging.Requests.Authors.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Requests.Authors.Application.GetAuthors;

public record GetAuthorsQuery(IQueryAuthorizer QueryAuthorizer);

public static class GetAuthorsHandler
{
    public static QueryableResponse<Author> Handle(GetAuthorsQuery request)
    {
        return new QueryableResponse<Author>(
            request.QueryAuthorizer.GetAuthorizedEntities<Author>());
    }
}