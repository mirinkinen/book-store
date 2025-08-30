using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Application.GetBooksFromAuthor;

public record GetBooksFromAuthorQuery(Guid AuthorId, IQueryAuthorizer<Book> QueryAuthorizer);

public static class GetBooksFromAuthorHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBooksFromAuthorQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = request.QueryAuthorizer.GetQuery(user);
        query = query.Where(b => b.AuthorId == request.AuthorId);

        return new QueryableResponse<Book>(query);
    }
}