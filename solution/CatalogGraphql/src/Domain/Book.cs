using Common.Domain;

namespace Domain;

public class Book : Entity
{
    public string Title { get; set; }

    public DateOnly DatePublished { get; set; }

    public Author Author { get; set; }

    public Guid AuthorId { get; set; }

    public decimal Price { get; set; }

    [Obsolete("Only for serialization", true)]
    public Book()
    {
    }

    public void SetAuthor(Author author)
    {
        Author = author ?? throw new ArgumentNullException(nameof(author));
        AuthorId = author.Id;
    }

    public Book(Guid authorId, string title, DateOnly datePublished, decimal price)
    {
        AuthorId = authorId;
        Title = title;
        DatePublished = datePublished;
        Price = price;
    }
}