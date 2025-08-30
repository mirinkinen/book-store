using Domain;
using API.Types.Mapping;

namespace API.Types;

[QueryType]
public static class Query
{
    public static BookDto GetBook()
    {
        var author = new Author("Jon", "Skeet", DateTime.UtcNow.AddYears(-30), Guid.NewGuid());
        var book = new Book(author.Id, "C# in depth", DateTime.UtcNow.AddYears(-10), 35);
        book.Author = author;

        // Map domain model to GraphQL DTO
        return book.ToDto();
    }
}