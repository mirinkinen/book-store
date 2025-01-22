using Cataloging.Domain;
using Common.Application;

namespace Cataloging.Application.GetAuthors;

public record GetAuthorsQuery(IQueryAuthorizer QueryAuthorizer);

public static class GetAuthorsHandler
{
    public static async Task<QueryableResponse<Author>> Handle(GetAuthorsQuery request)
    {
        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Author>();
        return new QueryableResponse<Author>(query);
    }
}