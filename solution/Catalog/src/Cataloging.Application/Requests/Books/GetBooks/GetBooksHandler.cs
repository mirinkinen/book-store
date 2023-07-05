using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.Requests.Books.GetBooks;

public record GetBooksQuery(User Actor, IQueryAuthorizer QueryAuthorizer);

public static class GetBooksHandler
{
    public static QueryableResponse<Book> Handle(GetBooksQuery request)
    {
        return new QueryableResponse<Book>(
            request.QueryAuthorizer.GetAuthorizedEntities<Book>());
    }
}