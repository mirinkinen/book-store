using Cataloging.Domain;
using Common.Domain;
using FluentAssertions;

namespace Cataloging.UnitTests.Domain.Books;

[Trait("Category", "Book")]
public class BookTests
{
    [Fact]
    public void Book_WhenCreated_HasBasicInformation()
    {
        var title = "Test book";
        var authorId = Guid.NewGuid();
        var datePublished = new DateTime(2020, 1, 1);
        var book = new Book(authorId, title, datePublished, 10);

        book.Title.Should().Be(title);
        book.DatePublished.Should().Be(datePublished);
        book.AuthorId.Should().Be(authorId);
    }

    [Fact]
    public void Book_WhenCreated_ExtendsEntity()
    {
        var book = new Book(Guid.NewGuid(), "test", DateTime.Now, 10);

        book.GetType().Should().BeAssignableTo<Entity>();
    }
}