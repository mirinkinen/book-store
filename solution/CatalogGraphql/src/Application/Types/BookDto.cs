using HotChocolate.Types;

namespace Application.Types;

[ObjectType("Book")]
public class BookDto
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Title { get; set; }
    public DateTime DatePublished { get; set; }
    public decimal Price { get; set; }
}