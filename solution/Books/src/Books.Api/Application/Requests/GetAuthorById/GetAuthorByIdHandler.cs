using Books.Api.Domain.Authors;
using Books.Api.Infrastructure.Database;
using MediatR;

namespace Books.Api.Application.Requests.GetAuthorById;

public record GetAuthorByIdQuery(Guid AuthorId) : IRequest<Author?>;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, Author?>
{
    private readonly BooksDbContext _booksDbContext;

    public GetAuthorByIdHandler(BooksDbContext booksDbContext)
    {
        _booksDbContext = booksDbContext;
    }

    public Task<Author?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        return _booksDbContext.Authors
            .FindAsync(new object[] { request.AuthorId }, cancellationToken: cancellationToken)
            .AsTask();
    }
}