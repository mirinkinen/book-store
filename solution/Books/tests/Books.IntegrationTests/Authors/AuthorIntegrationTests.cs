using FluentAssertions;
using System.Net.Http.Json;

namespace Books.IntegrationTests.Authors;

[Trait("Category", "Author")]
public class AuthorIntegrationTests : IntegrationTest
{
    [Fact]
    public async Task Get_Top3_Returns3Authors()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("v1/authors?$top=3");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();
        odata.Value.Should().HaveCount(3);
    }

    [Fact]
    public async Task Get_Select3Properties_Returns3Properties()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("v1/authors?$top=3&$select=id,firstname,lastname");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();
        var authors = odata.Value;
        authors.Should().HaveCount(3);

        var author = authors.First();

        // Selected fields should not be empty.
        author.Id.Should().NotBeNull().And.NotBeEmpty();
        author.FirstName.Should().NotBeNull().And.NotBeEmpty();
        author.LastName.Should().NotBeNull().And.NotBeEmpty();

        // Unselected fields should be null.
        author.Birthday.Should().BeNull();
    }

    [Fact]
    public async Task Get_FilterByFirstName_ReturnsFilteredAuthors()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("v1/authors?$filter=contains(firstname,'n')");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();
        var authors = odata.Value;
        authors.Should().NotBeEmpty();

        authors.Should().OnlyContain(author => author.FirstName.Contains('n'));
    }

    [Fact]
    public async Task Get_AuthorById_ReturnsAuthor()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");
        var authorId = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");

        // Act
        var response = await client.GetAsync($"v1/authors({authorId})");

        // Assert
        var author = await response.Content.ReadFromJsonAsync<AuthorViewmodel>();
        author.Should().NotBeNull();
        author.Id.Should().Be(authorId);
    }

    [Fact]
    public async Task Get_AuthorWithBooks_ReturnsAuthorAndBooks()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");
        var authorId = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");

        // Act
        var response = await client.GetAsync($"v1/authors({authorId})");

        // Assert
        var author = await response.Content.ReadFromJsonAsync<AuthorViewmodel>();
        author.Should().NotBeNull();
        author.Id.Should().Be(authorId);
    }

    [Fact]
    public async Task Get_OrderByTitleAscending_ReturnsOrderedAuthors()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");
        var authorId = Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");

        // Act
        var response = await client.GetAsync($"v1/authors?$top=20&orderby=firstname");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();

        var authors = odata.Value;
        authors.Should().NotBeEmpty();
        authors.Should().BeInAscendingOrder(author => author.FirstName);
    }

    [Fact]
    public async Task Get_OrderByTitleDescending_ReturnsOrderedAuthors()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");
        var authorId = Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");

        // Act
        var response = await client.GetAsync($"v1/authors?$top=20&orderby=firstname desc");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();

        var authors = odata.Value;
        authors.Should().NotBeEmpty();
        authors.Should().BeInDescendingOrder(author => author.FirstName);
    }

    [Fact]
    public async Task Get_Count_ReturnsCount()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/authors/$count");

        // Assert
        var count = await response.Content.ReadFromJsonAsync<int>();
        count.Should().NotBe(0);
    }

    [Fact]
    public async Task Get_AuthorsWithBooks_ReturnsAuthorsWithBooks()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/authors?$top=1&$expand=books");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();

        var authors = odata.Value;
        authors.Should().NotBeEmpty();
        var author = authors.First();

        author.Should().NotBeNull();
        author.Books.Should().NotBeNull();
        author.Books.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Get_WithoutParameters_ReturnsOnePageOfAuthors()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/authors");

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();

        var authors = odata.Value;
        authors.Should().HaveCountLessThanOrEqualTo(20);
    }

    [Fact]
    public async Task Get_TopTooBig_Fails()
    {
        // Arrange
        var client = Factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/authors?$top=21");

        // Assert
        //var content = await response.Content.ReadAsStringAsync();
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error.Error.Message.Should().Contain("The limit of '20' for Top query has been exceeded");
    }
}