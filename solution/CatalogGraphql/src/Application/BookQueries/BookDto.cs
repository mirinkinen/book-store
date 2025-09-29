using Application.AuthorQueries;
using Domain;
using HotChocolate;
using System.Linq.Expressions;

namespace Application.BookQueries;

public class BookDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public DateOnly DatePublished { get; set; }

    public Guid AuthorId { get; set; }

    public AuthorDto Author { get; set; }

    public decimal Price { get; set; }
}

public static class BookExtensions
{
    public static BookDto ToDto(this Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            DatePublished = book.DatePublished,
            AuthorId = book.AuthorId,
            Price = book.Price
        };
    }

    // Expression-based projection for EF Core
    public static Expression<Func<Book, BookDto>> ToDtoExpression()
    {
        return book => new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            DatePublished = book.DatePublished,
            AuthorId = book.AuthorId,
            Price = book.Price
        };
    }
}