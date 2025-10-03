using Domain;
using HotChocolate;

namespace Application.BookQueries;

[GraphQLName("Book")]
public class BookNode
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public DateOnly DatePublished { get; set; }

    public Guid AuthorId { get; set; }

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