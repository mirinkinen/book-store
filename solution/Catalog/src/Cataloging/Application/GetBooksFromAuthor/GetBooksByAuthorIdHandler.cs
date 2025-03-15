using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.GetBooksFromAuthor;

public record GetBooksFromAuthorQuery(Guid AuthorId, IQueryAuthorizer QueryAuthorizer);

public static class GetBooksFromAuthorHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBooksFromAuthorQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Book>(user);
        query = query.Where(b => b.AuthorId == request.AuthorId);

        return new QueryableResponse<Book>(query);
    }
}