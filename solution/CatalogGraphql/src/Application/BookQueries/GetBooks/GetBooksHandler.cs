using Application.Types;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBooks;

public record GetBooksQuery : IRequest<IQueryable<BookDto>>;

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IQueryable<BookDto>>
{
    private readonly IReadRepository<Book> _readRepository;

    public GetBooksHandler(IReadRepository<Book> readRepository)
    {
        _readRepository = readRepository;
    }

    public Task<IQueryable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = _readRepository.GetQuery();

        return Task.FromResult(books.Select(book => book.ToDto()));
    }
}