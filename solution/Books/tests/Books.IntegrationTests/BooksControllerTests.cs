using Books.Domain.Books;
using Books.IntegrationTests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace Books.Api.Tests;

[Trait("Category", "Books")]
[Trait("Category", "Integration")]
public class BooksControllerTests : IClassFixture<ApiTestWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BooksControllerTests(ApiTestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_Top3_ShouldReturn3Books()
    {
        // Arrange
        await TestDataSeeder.SeedTestData(_factory);
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("odata/books?$top=3");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ODataResponse<Book>>();
        odata.Should().NotBeNull();
        odata.Value.Should().HaveCount(3);
    }
}