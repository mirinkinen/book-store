using Books.Domain.Authors;
using Books.IntegrationTests.Authors;

namespace Books.IntegrationTests.Books;

internal class BookViewmodel : EntityViewmodel
{
    public string? Title { get; set; }

    public DateTime? DatePublished { get; set; }

    public AuthorViewmodel? Author { get; set; }

    public Guid? AuthorId { get; set; }
}