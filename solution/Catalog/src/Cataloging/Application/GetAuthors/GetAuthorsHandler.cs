using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.GetAuthors;

public record GetAuthorsQuery(IReadOnlyDbContext ReadOnlyDbContext);

public static class GetAuthorsHandler
{
    public static async Task<QueryableResponse<Author>> Handle(GetAuthorsQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        return new QueryableResponse<Author>(request.ReadOnlyDbContext.GetAuthors(user));
    }
}