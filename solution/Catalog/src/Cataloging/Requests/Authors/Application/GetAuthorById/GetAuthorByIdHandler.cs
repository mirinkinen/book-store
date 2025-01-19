using Cataloging.Application;
using Cataloging.Requests.Authors.Domain;
using Common.Application;
using Common.Application.Authentication;

namespace Cataloging.Requests.Authors.Application.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId, IQueryAuthorizer QueryAuthorizer);

public static class GetAuthorByIdHandler
{
    public static QueryableResponse<Author> Handle(GetAuthorByIdQuery request)
    {
        return new QueryableResponse<Author>(
            request.QueryAuthorizer.GetAuthorizedEntities<Author>()
                .Where(a => a.Id == request.AuthorId));
    }
}