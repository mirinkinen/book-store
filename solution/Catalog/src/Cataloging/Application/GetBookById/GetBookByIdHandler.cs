using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.GetBookById;

public record GetBookByIdQuery(Guid BookId, IReadOnlyDbContext ReadOnlyDbContext);

public static class GetBookByIdHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBookByIdQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = request.ReadOnlyDbContext.GetBooks(user);
        return new QueryableResponse<Book>(query.Where(a => a.Id == request.BookId));
    }
}