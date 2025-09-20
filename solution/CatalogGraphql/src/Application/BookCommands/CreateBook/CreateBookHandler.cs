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
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;

    public CreateBookHandler(IAuthorRepository authorRepository, IBookRepository bookRepository)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
    }
    
    public async Task<BookDto> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.FirstOrDefaultAsync(command.AuthorId);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {command.AuthorId} not found");
        }

        var book = new Book(command.AuthorId, command.Title, command.DatePublished, command.Price);
        book.SetAuthor(author);
        
        _bookRepository.Add(book);
        await _bookRepository.SaveChangesAsync(cancellationToken);
        
        return book.ToDto();
    }
}
