using Books.Domain.Books;
using Books.Domain.SeedWork;
using FluentAssertions;

namespace Books.Api.Tests.Domain.Books;

[Trait("Category", "Books")]
public class EntityTests
{
    [Fact]
    public void Book_WhenCreated_HasBasicInformation()
    {
        var title = "Test book";
        var authorId = Guid.NewGuid();
        var datePublished = new DateTime(2020, 1, 1);
        var book = new Book(title, datePublished, authorId);

        book.Title.Should().Be(title);
        book.DatePublished.Should().Be(datePublished);
        book.AuthorId.Should().Be(authorId);
    }

    [Fact]
    public void Book_WhenCreated_ExtendsEntity()
    {
        var book = new Book("test", DateTime.Now, Guid.NewGuid());

        book.GetType().Should().BeAssignableTo<Entity>();
    }
}