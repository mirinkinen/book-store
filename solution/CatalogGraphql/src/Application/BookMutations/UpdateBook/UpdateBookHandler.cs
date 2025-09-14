using Application.Repositories;
using Domain;
using MediatR;

namespace Application.BookMutations.UpdateBook;

public class UpdateBookInput : IRequest<BookUpdatedOutput>
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required DateTime DatePublished { get; set; }
    public required decimal Price { get; set; }
}

public class BookUpdatedOutput
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required DateTime DatePublished { get; set; }
    public required decimal Price { get; set; }
}

public class UpdateBookHandler : IRequestHandler<UpdateBookInput, BookUpdatedOutput>
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<BookUpdatedOutput> Handle(UpdateBookInput input, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(input.Id);
        if (book == null)
        {
            throw new ArgumentException($"Book with ID {input.Id} not found");
        }

        book.Title = input.Title;
        book.DatePublished = input.DatePublished;
        book.Price = input.Price;

        var updatedBook = await _bookRepository.UpdateAsync(book);
        
        return new BookUpdatedOutput
        {
            Id = updatedBook.Id,
            Title = updatedBook.Title,
            DatePublished = updatedBook.DatePublished,
            Price = updatedBook.Price
        };
    }
}
