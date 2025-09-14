using Application.Types;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBooks;

public class GetBooksQuery : IRequest<IEnumerable<BookOutputType>>
{
}

public class GetBooksHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookOutputType>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookOutputType>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync();

        return books.Select(book => new BookOutputType
        {
            Id = book.Id,
            AuthorId = book.AuthorId,
            Title = book.Title,
            DatePublished = book.DatePublished,
            Price = book.Price
        });
    }
}