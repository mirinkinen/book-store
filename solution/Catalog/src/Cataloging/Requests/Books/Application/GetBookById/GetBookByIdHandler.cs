using Cataloging.Application;
using Cataloging.Requests.Books.Domain;
using Common.Application;

namespace Cataloging.Requests.Books.Application.GetBookById;

public record GetBookByIdQuery(Guid BookId, IQueryAuthorizer QueryAuthorizer);

public static class GetBookByIdHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBookByIdQuery request)
    {
        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Book>();
        return new QueryableResponse<Book>(query.Where(a => a.Id == request.BookId));
    }
}