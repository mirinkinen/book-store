using Books.Api.Domain.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Books.Api.Application.Requests.GetBookById;

public record GetBookByIdQuery(Guid BookId) : IRequest<Book?>;

public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, Book?>
{
    private readonly QueryAuthorizer _queryAuthorizer;

    public GetBookByIdHandler(QueryAuthorizer queryAuthorizer)
    {
        _queryAuthorizer = queryAuthorizer;
    }

    public Task<Book?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _queryAuthorizer.GetAuthorizedEntities<Book>()
            .FirstOrDefaultAsync(a => a.Id == request.BookId, cancellationToken: cancellationToken);
    }
}