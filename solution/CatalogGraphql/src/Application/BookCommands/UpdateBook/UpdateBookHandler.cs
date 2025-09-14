using Application.Types;
using Domain;
using MediatR;

namespace Application.BookCommands.UpdateBook;

public class UpdateBookCommand : IRequest<BookOutputType>
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required DateTime DatePublished { get; set; }
    public required decimal Price { get; set; }
}

public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, BookOutputType>
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<BookOutputType> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(command.Id);
        if (book == null)
        {
            throw new ArgumentException($"Book with ID {command.Id} not found");
        }

        book.Title = command.Title;
        book.DatePublished = command.DatePublished;
        book.Price = command.Price;

        var updatedBook = await _bookRepository.UpdateAsync(book);
        
        return new BookOutputType
        {
            Id = updatedBook.Id,
            AuthorId = updatedBook.AuthorId,
            Title = updatedBook.Title,
            DatePublished = updatedBook.DatePublished,
            Price = updatedBook.Price
        };
    }
}
