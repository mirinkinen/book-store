using Cataloging.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Application.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId, IQueryAuthorizer QueryAuthorizer);

public static class GetAuthorByIdHandler
{
    public static async Task<QueryableResponse<Author>> Handle(GetAuthorByIdQuery request, IUserAccessor userAccessor)
    {
        var user = await userAccessor.GetUser();
        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Author>(user);
        return new QueryableResponse<Author>(query.Where(a => a.Id == request.AuthorId));
    }
}