using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Books.Api.Tests;

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
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/odata/books");

        var odata = await response.Content.ReadFromJsonAsync<JsonObject>();
    }
}