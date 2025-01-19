using Cataloging.Application;
using Cataloging.Requests.Authors.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Requests.Authors.Application.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId, IQueryAuthorizer QueryAuthorizer);

public static class GetAuthorByIdHandler
{
    public static async Task<QueryableResponse<Author>> Handle(GetAuthorByIdQuery request)
    {
        var query = await request.QueryAuthorizer.GetAuthorizedEntities<Author>();
        return new QueryableResponse<Author>(query.Where(a => a.Id == request.AuthorId));
    }
}