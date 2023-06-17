using Books.Api.Domain.Authors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.Application.Requests.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId) : IRequest<Author?>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, Author?>
{
    private readonly QueryAuthorizer _queryAuthorizer;

    public GetAuthorByIdHandler(QueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<Author?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _queryAuthorizer.GetAuthorizedEntities<Author>()
            .FirstOrDefaultAsync(a => a.Id == request.AuthorId, cancellationToken: cancellationToken);
    }
}