using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.GetBookById;

public record GetBookByIdQuery(Guid BookId, IQueryAuthorizer QueryAuthorizer);

public static class GetBookByIdHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBookByIdQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Book>(user);
        return new QueryableResponse<Book>(query.Where(a => a.Id == request.BookId));
    }
}