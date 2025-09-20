using Application.Types;
using Domain;
using MediatR;

namespace Application.BookCommands.UpdateBook;

public record UpdateBookCommand(
    Guid Id,
    string Title,
    DateOnly DatePublished,
    decimal Price) : IRequest<BookDto>;

public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IBookWriteRepository _bookWriteRepository;

    public UpdateBookHandler(IBookWriteRepository bookWriteRepository)
    {
        _bookWriteRepository = bookWriteRepository;
    }
    
    public async Task<BookDto> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
    {
        var book = await _bookWriteRepository.FirstOrDefaultAsync(command.Id);
        if (book == null)
        {
            throw new ArgumentException($"Book with ID {command.Id} not found");
        }

        book.Title = command.Title;
        book.DatePublished = command.DatePublished;
        book.Price = command.Price;

        await _bookWriteRepository.SaveChangesAsync(cancellationToken);

        return book.ToDto();
    }
}
