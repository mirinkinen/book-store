using Books.Api.Domain.Books;
using Books.Api.Infrastructure.Database;
using MediatR;

namespace Books.Api.Application.Requests.GetBooks;

public class GetBooksQuery : IRequest<IQueryable<Book>>
{
}

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IQueryable<Book>>
{
    private readonly BooksDbContext _booksDbContext;

    public GetBooksHandler(BooksDbContext booksDbContext)
    {
        _booksDbContext = booksDbContext;
    }

    public Task<IQueryable<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_booksDbContext.Books.AsQueryable());
    }

}