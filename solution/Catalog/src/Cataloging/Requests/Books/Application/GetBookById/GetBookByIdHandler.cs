using Cataloging.Application;
using Cataloging.Requests.Books.Domain;
using Common.Api.Application;

namespace Cataloging.Requests.Books.Application.GetBookById;

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