using FluentAssertions;
using System.Net.Http.Json;

namespace Books.IntegrationTests.Books;

[Trait("Category", "Books")]
[Trait("Category", "Integration")]
public class BooksControllerTests : DatabaseTest
{
    [Fact]
    public async Task Get_Top3_ShouldReturn3Books()
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
    }

    [Fact]
    public async Task Get_Select3Properties_ShouldReturn3Properties()
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
    public async Task Get_FilterByTitle_ShouldReturnFilteredBooks()
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
    public async Task Get_BookById_ReturnsBookById()
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
    public async Task Get_OrderByTitleAscending_ReturnsBooksOrderedByTitle()
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
    public async Task Get_OrderByTitleDescending_ReturnsBooksOrderedByTitle()
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
    public async Task Get_ExpandAuthors_ReturnsBooksWithAuthors()
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
    public async Task Get_Returns20Books()
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