using Cataloging.Application;
using Cataloging.Requests.Books.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Requests.Books.Application.GetBooksFromAuthor;

public record GetBooksFromAuthorQuery(Guid AuthorId, IQueryAuthorizer QueryAuthorizer);

public static class GetBooksFromAuthorHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBooksFromAuthorQuery request)
    {
        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Book>();
        query = query.Where(b => b.AuthorId == request.AuthorId);

        return new QueryableResponse<Book>(query);
    }
}