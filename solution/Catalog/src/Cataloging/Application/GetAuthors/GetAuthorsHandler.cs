using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.GetAuthors;

public record GetAuthorsQuery(IQueryAuthorizer QueryAuthorizer);

public static class GetAuthorsHandler
{
    public static async Task<QueryableResponse<Author>> Handle(GetAuthorsQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Author>(user);
        return new QueryableResponse<Author>(query);
    }
}