using Books.Application.Services;
using Books.Domain.Authors;
using MediatR;
using Microsoft.AspNetCore.OData.Results;

namespace Books.Application.Requests.Authors.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId) : IRequest<SingleResult<Author>>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, SingleResult<Author>>
{
    private readonly IQueryAuthorizer _queryAuthorizer;

    public GetAuthorByIdHandler(IQueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<SingleResult<Author>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var query = _queryAuthorizer.GetAuthorizedEntities<Author>().Where(a => a.Id == request.AuthorId);

        return Task.FromResult(SingleResult.Create(query));
    }
}