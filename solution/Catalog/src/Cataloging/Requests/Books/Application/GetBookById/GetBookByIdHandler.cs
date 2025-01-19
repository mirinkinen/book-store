using Cataloging.Application;
using Cataloging.Requests.Books.Domain;
using Common.Application;

namespace Cataloging.Requests.Books.Application.GetBookById;

public record GetBookByIdQuery(Guid BookId, IQueryAuthorizer QueryAuthorizer);

public static class GetBookByIdHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBookByIdQuery request)
    {
        return new QueryableResponse<Book>(
            (await request.QueryAuthorizer.GetAuthorizedEntities<Book>())
                .Where(a => a.Id == request.BookId));
    }
}