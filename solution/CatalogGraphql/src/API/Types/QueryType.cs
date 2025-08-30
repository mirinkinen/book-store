using Domain;

namespace API.Types;

[QueryType]
public class Query
{
    public Book GetBook()
    {
        var author = new Author("Jon", "Skeet", DateTime.UtcNow.AddYears(-30), Guid.NewGuid());
        var book = new Book(author.Id, "C# in depth", DateTime.UtcNow.AddYears(-10), 35);
        book.Author = author;

        return book;
    }
}