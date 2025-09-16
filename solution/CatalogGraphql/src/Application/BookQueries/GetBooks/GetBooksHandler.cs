using Application.Types;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBooks;

public class GetBooksQuery : IRequest<IEnumerable<BookDto>>
{
}

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync();

        return books.Select(book => book.ToDto());
    }
}