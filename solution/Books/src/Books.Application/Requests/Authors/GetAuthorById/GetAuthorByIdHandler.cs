using Books.Application.Auditing;
using Books.Application.Services;
using Books.Domain.Authors;
using MediatR;

namespace Books.Application.Requests.Authors.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId, User Actor) : IAuditRequest<IQueryable<Author>>
{
    public OperationType OperationType => OperationType.Read;
}

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, IQueryable<Author>>
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetAuthorByIdHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<IQueryable<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return Task.FromResult(_queryAuthorizer.GetAuthorizedEntities<Author>().Where(a => a.Id == request.AuthorId));
    }
}