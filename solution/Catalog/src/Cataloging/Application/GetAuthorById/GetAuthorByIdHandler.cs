using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId, IReadOnlyDbContext ReadOnlyDbContext);

public static class GetAuthorByIdHandler
{
    public static async Task<QueryableResponse<Author>> Handle(GetAuthorByIdQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = request.ReadOnlyDbContext.GetAuthors(user).Where(a => a.Id == request.AuthorId);
        
        return new QueryableResponse<Author>(query);
    }
}