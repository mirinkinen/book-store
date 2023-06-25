using Books.Api.Tests;
using FluentAssertions;
using System.Net.Http.Json;

namespace Books.IntegrationTests.Authors;

[Trait("Category", "Authors")]
[Trait("Category", "Integration")]
public class AuthorsControllerTests : DatabaseTest
{
    [Fact]
    public async Task Get_Top3_ShouldReturn3Authors()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("odata/authors?$top=3");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();
        odata.Value.Should().HaveCount(3);
    }
}