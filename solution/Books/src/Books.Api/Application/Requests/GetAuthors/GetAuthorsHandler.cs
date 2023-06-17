using Books.Api.Domain.Authors;
using MediatR;

namespace Books.Api.Application.Requests.GetAuthors;

public class GetAuthorsQuery : IRequest<IQueryable<Author>>
{
}

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IQueryable<Author>>
{
    private readonly QueryAuthorizer _queryAuthorizer;

    public GetAuthorsHandler(QueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Author>());
    }
}