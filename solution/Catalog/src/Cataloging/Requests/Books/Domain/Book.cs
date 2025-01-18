using Cataloging.Domain;
using Cataloging.Requests.Authors.Domain;

namespace Cataloging.Requests.Books.Domain;

public class Book : Entity
{
    public string Title { get; protected set; }

    public DateTime DatePublished { get; protected set; }

    public Author Author { get; protected set; }

    public Guid AuthorId { get; protected set; }

    public decimal Price { get; protected set; }

    [Obsolete("Only for serialization", true)]
    public Book()
    {
    }

    public Book(Guid authorId, string title, DateTime datePublished, decimal price)
    {
        AuthorId = authorId;
        Title = title;
        DatePublished = datePublished;
        Price = price;
    }
}