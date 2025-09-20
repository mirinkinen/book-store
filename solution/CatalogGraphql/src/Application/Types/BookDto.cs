using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace Application.Types;

[ObjectType("Book")]
public record BookDto
{
    public BookDto(Guid id,
        Guid authorId,
        string title,
        DateOnly datePublished,
        decimal price)
    {
        Id = id;
        AuthorId = authorId;
        Title = title;
        DatePublished = datePublished;
        Price = price;
    }
    
    [Obsolete("Only for serialization", true)]
    public BookDto() {}

    public Guid Id { get; init; }
    
    [ID]
    public Guid AuthorId { get; init; }
    public string Title { get; init; }
    public DateOnly DatePublished { get; init; }
    public decimal Price { get; init; }
}