using Application.BookQueries.GetBook;
using Application.Repositories;
using MediatR;

namespace Application.BookQueries.GetBooksByAuthor;

public class GetBooksByAuthorInput : IRequest<IEnumerable<GetBookOutput>>
{
    public required Guid AuthorId { get; set; }
}

public class GetBooksByAuthorHandler : IRequestHandler<GetBooksByAuthorInput, IEnumerable<GetBookOutput>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksByAuthorHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<IEnumerable<GetBookOutput>> Handle(GetBooksByAuthorInput request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetByAuthorIdAsync(request.AuthorId);
        
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
