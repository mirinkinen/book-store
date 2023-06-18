using Books.Domain.Books;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Books.Api.Tests;

[Trait("Category", "Books")]
[Trait("Category", "API")]
public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BooksControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ShouldReturnsBooks()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("odata/books");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ODataResponse<Book>>();
        odata.Should().NotBeNull();
        odata.Value.Should().NotBeEmpty();
    }
}