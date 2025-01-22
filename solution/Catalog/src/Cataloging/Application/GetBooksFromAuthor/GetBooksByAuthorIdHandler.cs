using Cataloging.Domain;
using Common.Application;

namespace Cataloging.Application.GetBooksFromAuthor;

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