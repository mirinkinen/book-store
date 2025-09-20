using Application.Types;
using Domain;
using MediatR;

namespace Application.BookCommands.UpdateBook;

public record UpdateBookCommand(
    Guid Id,
    string Title,
    DateTime DatePublished,
    decimal Price) : IRequest<BookDto>;

public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IBookRepository _bookRepository;

    public UpdateBookHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    
    public async Task<BookDto> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FirstOrDefaultAsync(command.Id);
        if (book == null)
        {
            throw new ArgumentException($"Book with ID {command.Id} not found");
        }

        book.Title = command.Title;
        book.DatePublished = command.DatePublished;
        book.Price = command.Price;

        await _bookRepository.SaveChangesAsync(cancellationToken);

        return book.ToDto();
    }
}
