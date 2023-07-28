using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.Requests.Books.GetBooksFromAuthor;

public record GetBooksFromAuthorQuery(User Actor, Guid AuthorId, IQueryAuthorizer QueryAuthorizer);

public static class GetBooksFromAuthorHandler
{
    public static QueryableResponse<Book> Handle(GetBooksFromAuthorQuery request)
    {
        var query = request.QueryAuthorizer.GetAuthorizedEntities<Book>();
        query = query.Where(b => b.AuthorId == request.AuthorId);

        return new QueryableResponse<Book>(query);
    }
}