using Books.Domain.Books;
using Books.Domain.SeedWork;
using FluentAssertions;

namespace Books.Domain.UnitTests.Books;

[Trait("Category", "Book")]
public class BookTests
{
    [Fact]
    public void Book_WhenCreated_HasBasicInformation()
    {
        var modifiedBy = Guid.NewGuid();
        var title = "Test book";
        var authorId = Guid.NewGuid();
        var datePublished = new DateTime(2020, 1, 1);
        var book = new Book(title, datePublished, authorId, modifiedBy);

        book.Title.Should().Be(title);
        book.DatePublished.Should().Be(datePublished);
        book.AuthorId.Should().Be(authorId);
    }

    [Fact]
    public void Book_WhenCreated_ExtendsEntity()
    {
        var book = new Book("test", DateTime.Now, Guid.NewGuid(), Guid.NewGuid());

        book.GetType().Should().BeAssignableTo<Entity>();
    }
}