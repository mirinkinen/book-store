using Books.Api.Tests;
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
        var response = await client.GetAsync("odata/books?$top=3");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ODataResponse<BookViewmodel>>();
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
        var response = await client.GetAsync("odata/books?$top=3&$select=id,title,createdat");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ODataResponse<BookViewmodel>>();
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
}