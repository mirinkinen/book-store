using Application.Types;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBooks;

public record GetBooksQuery : IRequest<IQueryable<BookDto>>;

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IQueryable<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Task<IQueryable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = _bookRepository.GetQuery();

        return Task.FromResult(books.Select(book => book.ToDto()));
    }
}