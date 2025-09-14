using Application.Repositories;
using Domain;
using MediatR;

namespace Application.BookMutations.CreateBook;

public class CreateBookInput : IRequest<BookCreatedOutput>
{
    public required Guid AuthorId { get; set; }
    public required string Title { get; set; }
    public required DateTime DatePublished { get; set; }
    public required decimal Price { get; set; }
}

public class BookCreatedOutput
{
    public required Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public required string Title { get; set; }
    public required DateTime DatePublished { get; set; }
    public required decimal Price { get; set; }
}

public class CreateBookHandler : IRequestHandler<CreateBookInput, BookCreatedOutput>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;

    public CreateBookHandler(IAuthorRepository authorRepository, IBookRepository bookRepository)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
    }
    
    public async Task<BookCreatedOutput> Handle(CreateBookInput input, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(input.AuthorId);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {input.AuthorId} not found");
        }

        var book = new Book(input.AuthorId, input.Title, input.DatePublished, input.Price);
        book.SetAuthor(author);
        
        var createdBook = await _bookRepository.AddAsync(book);
        
        return new BookCreatedOutput
        {
            Id = createdBook.Id,
            AuthorId = createdBook.AuthorId,
            Title = createdBook.Title,
            DatePublished = createdBook.DatePublished,
            Price = createdBook.Price
        };
    }
}
