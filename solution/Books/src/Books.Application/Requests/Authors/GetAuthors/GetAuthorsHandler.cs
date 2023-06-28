using Books.Application.Auditing;
using Books.Application.Services;
using Books.Domain.Authors;
using MediatR;

namespace Books.Application.Requests.Authors.GetAuthors;

public record GetAuthorsQuery(User Actor) : IAuditRequest<IQueryable<Author>>
{
    public OperationType OperationType => OperationType.Read;
}

public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IQueryable<Author>>
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetAuthorsHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Author>());
    }
}