using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Application.GetBookById;

public record GetBookByIdQuery(Guid BookId, IQueryAuthorizer<Book> QueryAuthorizer);

public static class GetBookByIdHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBookByIdQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = request.QueryAuthorizer.GetQuery(user);
        return new QueryableResponse<Book>(query.Where(a => a.Id == request.BookId));
    }
}