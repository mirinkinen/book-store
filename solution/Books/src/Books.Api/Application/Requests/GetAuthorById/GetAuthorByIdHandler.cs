using Books.Api.Domain.Authors;
using MediatR;
using Microsoft.AspNetCore.OData.Results;

namespace Books.Api.Application.Requests.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId) : IRequest<SingleResult<Author>>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, SingleResult<Author>>
{
    private readonly QueryAuthorizer _queryAuthorizer;

    public GetAuthorByIdHandler(QueryAuthorizer queryAuthorizer)
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