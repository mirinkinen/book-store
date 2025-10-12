using Domain;
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
    /// <summary>
    /// Maps a book to a book node.
    /// </summary>
    /// <remarks>Use when expression is not required.</remarks>
    public static BookNode ToDto(this Book book)
    {
        return new BookNode
        {
            Id = book.Id,
            Title = book.Title,
            DatePublished = book.DatePublished,
            AuthorId = book.AuthorId,
            Price = book.Price
        };
    }
    
    /// <summary>
    /// Maps a book to a book node.
    /// </summary>
    /// <remarks>Use when expression is required, for example in EF Core queries.</remarks>
    public static Expression<Func<Book, BookNode>> ToNode()
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