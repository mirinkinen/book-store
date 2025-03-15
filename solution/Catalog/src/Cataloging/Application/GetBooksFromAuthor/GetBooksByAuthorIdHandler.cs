using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.GetBooksFromAuthor;

public record GetBooksFromAuthorQuery(Guid AuthorId, IReadOnlyDbContext ReadOnlyDbContext);

public static class GetBooksFromAuthorHandler
{
    public static async Task<QueryableResponse<Book>> Handle(GetBooksFromAuthorQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = request.ReadOnlyDbContext.GetBooks(user);
        query = query.Where(b => b.AuthorId == request.AuthorId);

        return new QueryableResponse<Book>(query);
    }
}