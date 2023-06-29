using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Shared.Application;
using Shared.Application.Auditing;
using Shared.Application.Authentication;

namespace Cataloging.Application.Requests.Authors.GetAuthors;

public record GetAuthorsQuery(User Actor) : IAuditableQuery;

public class GetAuthorsHandler
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetAuthorsHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public QueryableResponse<Author> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return new QueryableResponse<Author>(_queryAuthorizer.GetAuthorizedEntities<Author>());
    }
}