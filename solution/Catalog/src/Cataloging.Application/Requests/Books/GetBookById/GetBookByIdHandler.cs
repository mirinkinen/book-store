using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using Common.Application;

namespace Cataloging.Application.Requests.Books.GetBookById;

public record GetBookByIdQuery(Guid BookId, IQueryAuthorizer QueryAuthorizer);

public static class GetBookByIdHandler
{
    public static QueryableResponse<Book> Handle(GetBookByIdQuery request)
    {
        return new QueryableResponse<Book>(
            request.QueryAuthorizer.GetAuthorizedEntities<Book>()
                .Where(a => a.Id == request.BookId));
    }
}