namespace Cataloging.API.Models;

public class BookV3
{
    public Guid Id { get; protected set; }
    public Guid AuthorId { get; protected set; }
    public DateTime DatePublished { get; protected set; }
    public string Title { get; protected set; }
    public decimal Price { get; protected set; }
}