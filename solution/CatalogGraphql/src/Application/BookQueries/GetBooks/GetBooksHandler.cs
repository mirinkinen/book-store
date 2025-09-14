using Application.BookQueries.GetBook;
using Application.Repositories;
using MediatR;

namespace Application.BookQueries.GetBooks;

public class GetBooksInput : IRequest<IEnumerable<GetBookOutput>>
{
}

public class GetBooksHandler : IRequestHandler<GetBooksInput, IEnumerable<GetBookOutput>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<IEnumerable<GetBookOutput>> Handle(GetBooksInput request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync();
        
        return books.Select(book => new GetBookOutput
        {
            Id = book.Id,
            AuthorId = book.AuthorId,
            Title = book.Title,
            DatePublished = book.DatePublished,
            Price = book.Price
        });
    }
}
