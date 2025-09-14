using HotChocolate.Types;

namespace Application.BookMutations.CreateBook;

[ObjectType("Book")]
public class BookOutputType
{
    public required Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public required string Title { get; set; }
    public required DateTime DatePublished { get; set; }
    public required decimal Price { get; set; }
}