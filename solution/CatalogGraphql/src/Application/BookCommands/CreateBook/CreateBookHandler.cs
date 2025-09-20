using Application.Types;
using Domain;
using MediatR;

namespace Application.BookCommands.CreateBook;

public record CreateBookCommand(
    Guid AuthorId,
    string Title,
    DateOnly DatePublished,
    decimal Price) : IRequest<BookDto>;

public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IAuthorWriteRepository _authorWriteRepository;
    private readonly IBookWriteRepository _bookWriteRepository;

    public CreateBookHandler(IAuthorWriteRepository authorWriteRepository, IBookWriteRepository bookWriteRepository)
    {
        _authorWriteRepository = authorWriteRepository;
        _bookWriteRepository = bookWriteRepository;
    }
    
    public async Task<BookDto> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var author = await _authorWriteRepository.FirstOrDefaultAsync(command.AuthorId);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {command.AuthorId} not found");
        }

        var book = new Book(command.AuthorId, command.Title, command.DatePublished, command.Price);
        book.SetAuthor(author);
        
        _bookWriteRepository.Add(book);
        await _bookWriteRepository.SaveChangesAsync(cancellationToken);
        
        return book.ToDto();
    }
}
