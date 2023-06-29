using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Shared.Application;
using Shared.Application.Auditing;
using Shared.Application.Authentication;

namespace Cataloging.Application.Requests.Authors.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId, User Actor) : IAuditableQuery;

public class GetAuthorByIdHandler
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetAuthorByIdHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public QueryableResponse<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new QueryableResponse<Author>(_queryAuthorizer.GetAuthorizedEntities<Author>().Where(a => a.Id == request.AuthorId));
    }
}