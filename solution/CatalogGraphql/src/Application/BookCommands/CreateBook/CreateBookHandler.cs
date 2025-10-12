using Application.BookQueries;
using Domain;
using Domain.Authors;
using Domain.Books;
using MediatR;

namespace Application.BookCommands.CreateBook;

public record CreateBookCommand(
    Guid AuthorId,
    string Title,
    DateOnly DatePublished,
    decimal Price) : IRequest<BookNode>;

public class CreateBookHandler : IRequestHandler<CreateBookCommand, BookNode>
{
    private readonly IAuthorWriteRepository _authorWriteRepository;
    private readonly IBookWriteRepository _bookWriteRepository;
    private readonly Lazy<Func<Book, BookNode>> _compiledProjection;
    
    public CreateBookHandler(IAuthorWriteRepository authorWriteRepository, IBookWriteRepository bookWriteRepository)
    {
        _authorWriteRepository = authorWriteRepository;
        _bookWriteRepository = bookWriteRepository;
        _compiledProjection = new Lazy<Func<Book, BookNode>>(() => BookExtensions.ToNode().Compile());
    }

    public async Task<BookNode> Handle(CreateBookCommand command, CancellationToken cancellationToken)
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

        return _compiledProjection.Value(book);
    }
}