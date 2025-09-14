using Application.Repositories;
using Domain;
using MediatR;

namespace Application.BookQueries.GetBook;

public class GetBookInput : IRequest<GetBookOutput?>
{
    public required Guid Id { get; set; }
}

public class GetBookOutput
{
    public required Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public required string Title { get; set; }
    public required DateTime DatePublished { get; set; }
    public required decimal Price { get; set; }
}

public class GetBookHandler : IRequestHandler<GetBookInput, GetBookOutput?>
{
    private readonly IBookRepository _bookRepository;

    public GetBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<GetBookOutput?> Handle(GetBookInput request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id);
        
        if (book == null)
            return null;
            
        return new GetBookOutput
        {
            Id = book.Id,
            AuthorId = book.AuthorId,
            Title = book.Title,
            DatePublished = book.DatePublished,
            Price = book.Price
        };
    }
}
