using Common.Application.Auditing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Cataloging.IntegrationTests.Books;

[Trait("Category", "Book")]
public class BookIntegrationTests : IntegrationTest
{
    private readonly AuditContext _auditContext = new();

    public BookIntegrationTests(ITestOutputHelper output) : base(output)
    {
        Factory.ConfigureServices = (IServiceCollection services) =>
        {
            services.AddLogging(builder => builder.AddXUnit(Output));
            services.AddScoped<AuditContext>(sp => _auditContext);
        };
    }

    [Fact]
    public async Task Get_Top3_Returns3Books()
    {
        // Arrange

        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=nonep");

        // Act
        var response = await client.GetAsync("v1/books?$top=3");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        odata.Should().NotBeNull();
        odata.Value.Should().HaveCount(3);

        // Verify that 3 IDs are audit logged.
        //_auditContext.Resources.Should().HaveCount(3);
        //_auditContext.Resources.Should().OnlyContain(r => r.Type == ResourceType.Book);
        //var ids = odata.Value.Where(b => b.Id.HasValue).Select(b => b.Id.Value);
        //_auditContext.Resources.Select(t => t.Id).Should().ContainInConsecutiveOrder(ids);
        //_auditContext.OperationType.Should().Be(OperationType.Read);
    }

    [Fact]
    public async Task Get_Select3Properties_Returns3Properties()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("v1/books?$top=3&$select=id,title,createdat");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        odata.Should().NotBeNull();
        var books = odata.Value;
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
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("v1/books?$filter=contains(title,'and')");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        odata.Should().NotBeNull();
        var books = odata.Value;
        books.Should().NotBeEmpty();

        books.Should().OnlyContain(book => book.Title.Contains("and"));
    }

    [Fact]
    public async Task Get_BookById_ReturnsBook()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");
        var bookId = Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");

        // Act
        var response = await client.GetAsync($"v1/books({bookId})");

        // Assert
        var book = await response.Content.ReadFromJsonAsync<BookViewmodel>();
        book.Should().NotBeNull();
        book.Id.Should().Be(bookId);
    }

    [Fact]
    public async Task Get_OrderByTitleAscending_ReturnsOrderedBooks()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");
        var bookId = Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");

        // Act
        var response = await client.GetAsync($"v1/books?$top=20&orderby=title");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        odata.Should().NotBeNull();

        var books = odata.Value;
        books.Should().NotBeEmpty();
        books.Should().BeInAscendingOrder(book => book.Title);
    }

    [Fact]
    public async Task Get_OrderByTitleDescending_ReturnsOrderedBooks()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");
        var bookId = Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");

        // Act
        var response = await client.GetAsync($"v1/books?$top=20&orderby=title desc");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        odata.Should().NotBeNull();

        var books = odata.Value;
        books.Should().NotBeEmpty();
        books.Should().BeInDescendingOrder(book => book.Title);
    }

    [Fact]
    public async Task Get_Count_ReturnsCount()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/books/$count");

        // Assert
        var count = await response.Content.ReadFromJsonAsync<int>();
        count.Should().NotBe(0);
    }

    [Fact]
    public async Task Get_BooksWithAuthors_ReturnsBooksWithAuthors()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/books?$top=1&$expand=author");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        odata.Should().NotBeNull();

        var books = odata.Value;
        books.Should().NotBeEmpty();
        var book = books.First();

        book.Should().NotBeNull();
        book.Author.Should().NotBeNull();
        book.Author.Id.Should().NotBeNull();
    }

    [Fact]
    public async Task Get_WihtoutParameters_ReturnsOnePageOfBooks()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/books");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<BookViewmodel>>();
        odata.Should().NotBeNull();

        var books = odata.Value;
        books.Should().HaveCount(20);
    }

    [Fact]
    public async Task Get_Top21_Fails()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/books?$top=21");

        // Assert
        //var content = await response.Content.ReadAsStringAsync();
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error.Error.Message.Should().Contain("The limit of '20' for Top query has been exceeded");
    }
}