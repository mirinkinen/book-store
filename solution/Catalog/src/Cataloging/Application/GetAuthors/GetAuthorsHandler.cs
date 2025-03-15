using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Application.GetAuthors;

public record GetAuthorsQuery(IQueryAuthorizer<Author> QueryAuthorizer);

public static class GetAuthorsHandler
{
    public static async Task<QueryableResponse<Author>> Handle(GetAuthorsQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        return new QueryableResponse<Author>(request.QueryAuthorizer.GetQuery(user));
    }
}