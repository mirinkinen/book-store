using Books.Api.Domain.Books;
using Books.Api.Infrastructure.Database;
using MediatR;

namespace Books.Api.Application.Requests.GetBooks;

public class GetBooksQuery : IStreamRequest<Book>
{
}

public class GetBooksHandler : IStreamRequestHandler<GetBooksQuery, Book>
{
    private readonly BooksDbContext _booksDbContext;

    public GetBooksHandler(BooksDbContext booksDbContext)
    {
        _booksDbContext = booksDbContext;
    }

    public IAsyncEnumerable<Book> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return _booksDbContext.Books.AsAsyncEnumerable();
    }
}