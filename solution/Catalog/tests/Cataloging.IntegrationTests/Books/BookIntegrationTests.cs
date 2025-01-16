using Common.Api.Application.Auditing;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using FluentAssertions;
using Wolverine.Tracking;

namespace Cataloging.IntegrationTests.Books;

[Trait("Category", "Book")]
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable",
    Justification = "Disposed via IAsyncLifetime")]
public class BookIntegrationTests : IntegrationContext
{
    public BookIntegrationTests(AppFixture app) : base(app)
    {
    }

    [Fact]
    public async Task Get_Top3_Returns3Books()
    {
        // Arrange
        ValueResponse<BookViewmodel>? content = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books?$top=3");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();
        content.Value.Should().HaveCount(3);

        // Verify that 3 IDs are audit logged.
        var auditLogEvent = tracked.FindSingleTrackedMessageOfType<AuditLogEvent>();

        auditLogEvent.Resources.Should().HaveCount(3);
        auditLogEvent.Resources.Should().OnlyContain(r => r.ResourceType == "Book");
        var ids = content.Value.Where(b => b.Id.HasValue).Select(b => b.Id.Value);
        auditLogEvent.Resources.Select(t => t.ResourceId).Should().ContainInConsecutiveOrder(ids);
    }

    [Fact]
    public async Task Get_Select3Properties_Returns3Properties()
    {
        // Arrange
        ValueResponse<BookViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books?$top=3&$select=id,title,createdat");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();
        var books = content.Value;
        books.Should().HaveCount(3);

        var book = books.First();

        // Selected fields should not be empty.
        book.Id.Should().NotBeNull().And.NotBeEmpty();
        book.Title.Should().NotBeNull().And.NotBeEmpty();
        book.CreatedAt.Should().NotBeNull().And.NotBe(DateTime.MinValue);

        // Unselected fields should be null.
        book.DatePublished.Should().BeNull();
    }

    [Fact]
    public async Task Get_FilterByTitle_ReturnsFilteredBooks()
    {
        // Arrange
        ValueResponse<BookViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books?$filter=contains(title,'and')");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();
        var books = content.Value;
        books.Should().NotBeEmpty();
        books.Should().OnlyContain(book => book.Title.Contains("and"));
    }

    [Fact]
    public async Task Get_BookById_ReturnsBook()
    {
        // Arrange
        BookViewmodel? book = null;
        var bookId = Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");


        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync($"v1/books({bookId})");
            book = await response.Content.ReadFromJsonAsync<BookViewmodel>();
        });

        // Assert
        book.Should().NotBeNull();
        book.Id.Should().Be(bookId);
    }

    [Fact]
    public async Task Get_OrderByTitleAscending_ReturnsOrderedBooks()
    {
        // Arrange
        ValueResponse<BookViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books?$top=20&orderby=title");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();

        var books = content.Value;
        books.Should().NotBeEmpty();
        books.Should().BeInAscendingOrder(book => book.Title);
    }

    [Fact]
    public async Task Get_OrderByTitleDescending_ReturnsOrderedBooks()
    {
        // Arrange
        ValueResponse<BookViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books?$top=20&orderby=title desc");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();

        var books = content.Value;
        books.Should().NotBeEmpty();
        books.Should().BeInDescendingOrder(book => book.Title);
    }

    [Fact]
    public async Task Get_Count_ReturnsCount()
    {
        // Arrange
        var count = 0;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books/$count");
            count = await response.Content.ReadFromJsonAsync<int>();
        });

        // Assert
        count.Should().NotBe(0);
    }

    [Fact]
    public async Task Get_BooksWithAuthors_ReturnsBooksWithAuthors()
    {
        // Arrange
        ValueResponse<BookViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books?$top=1&$expand=author");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();

        var books = content.Value;
        books.Should().NotBeEmpty();
        var book = books.First();

        book.Should().NotBeNull();
        book.Author.Should().NotBeNull();
        book.Author.Id.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_WithoutParameters_ReturnsOnePageOfBooks()
    {
        // Arrange
        ValueResponse<BookViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();

        var books = content.Value;
        books.Should().HaveCount(20);
    }

    [Fact]
    public async Task Get_Top21_Fails()
    {
        // Arrange
        ErrorResponse? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("v1/books?$top=21");
            content = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        });

        // Assert
        content.Should().NotBeNull();
        content.Error.Message.Should().Contain("The limit of '20' for Top query has been exceeded");
    }
}