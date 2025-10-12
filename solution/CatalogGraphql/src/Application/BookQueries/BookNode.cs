using Domain;
using Domain.Books;
using HotChocolate;
using System.Linq.Expressions;

namespace Application.BookQueries;

[GraphQLName("Book")]
public class BookNode
{
    /// <summary>
    /// ID of the book.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title of the book.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Publication date of the book.
    /// </summary>
    public DateOnly DatePublished { get; set; }

    /// <summary>
    /// ID of the author.
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Price of the book.
    /// </summary>
    public decimal Price { get; set; }
}

public static class BookExtensions
{
    private static readonly Lazy<Func<Book, BookNode>> _compiledProjection = new(() => ProjectToNode().Compile());
    
    /// <summary>
    /// Maps a book to a book node.
    /// </summary>
    public static BookNode MapToDto(this Book book)
    {
        return _compiledProjection.Value(book);
    }
    
    /// <summary>
    /// Projects a book to a book node.
    /// </summary>
    public static Expression<Func<Book, BookNode>> ProjectToNode()
    {
        return book => new BookNode
        {
            Id = book.Id,
            Title = book.Title,
            DatePublished = book.DatePublished,
            AuthorId = book.AuthorId,
            Price = book.Price
        };
    }
}