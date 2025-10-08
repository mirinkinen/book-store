using Domain;
using HotChocolate;

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
}