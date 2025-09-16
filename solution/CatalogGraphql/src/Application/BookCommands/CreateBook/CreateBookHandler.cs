using Application.Types;
using Domain;
using MediatR;

namespace Application.BookCommands.CreateBook;

public class CreateBookCommand : IRequest<BookDto>
{
    public required Guid AuthorId { get; set; }
    public required string Title { get; set; }
    public required DateTime DatePublished { get; set; }
    public required decimal Price { get; set; }
}

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
        var author = await _authorRepository.GetByIdAsync(command.AuthorId);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {command.AuthorId} not found");
        }

        var book = new Book(command.AuthorId, command.Title, command.DatePublished, command.Price);
        book.SetAuthor(author);
        
        var createdBook = await _bookRepository.AddAsync(book);
        
        return createdBook.ToDto();
    }
}
