using Books.Api.Domain.Books;
using FluentAssertions;

namespace Books.Api.Tests.Domain.Books
{
    public class BooksTests
    {
        [Fact]
        public void Book_WhenCreated_HasBasicEntityInformation()
        {
            var title = "Test book";
            var authorId = Guid.NewGuid();
            DateTime datePublished = new DateTime(2020, 1, 1);
            var book = new Book(title, datePublished, authorId);

            book.Id.Should().NotBeEmpty();
            book.Created.Should().NotBe(DateTimeOffset.MinValue);
            book.Updated.Should().NotBe(DateTimeOffset.MinValue);
            book.Deleted.Should().BeNull();
            book.Title.Should().Be(title);
            book.DatePublished.Should().Be(datePublished);
            book.AuthorId.Should().Be(authorId);
        }
    }
}