using System.Diagnostics.CodeAnalysis;
using Cataloging.Api.Authors;
using Cataloging.Application.Requests.Authors.AddAuthor;
using Cataloging.Infrastructure.Database;
using Cataloging.IntegrationTests.Fakes;
using Common.Application.Auditing;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Cataloging.IntegrationTests.Authors;

[Trait("Category", "Author")]
public class AuthorIntegrationTests : IAsyncDisposable
{
    private readonly IntegrationWebApplicationFactory _factory;
    private readonly AuditContext _auditContext = new();

    public AuthorIntegrationTests(ITestOutputHelper output)
    {
        _factory = new IntegrationWebApplicationFactory();
        _factory.ConfigureServices = (services) =>
        {
            services.AddLogging(builder => builder.AddXUnit(output));
            services.AddScoped<AuditContext>(sp => _auditContext);
        };
    }

    [Fact]
    public async Task Get_Top3_Returns3Authors()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync("v1/authors?$top=3");

        // TODO: Refactor this test.
        await Task.Delay(100);

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();
        odata.Value.Should().HaveCount(3);

        // Verify audit logging.
        _auditContext.Resources.Should().HaveCount(3);
        _auditContext.Resources.Should().OnlyContain(alr => alr.ResourceType == "Author" && alr.ResourceId != Guid.Empty);
    }

    [Fact]
    public async Task Get_Select3Properties_Returns3Properties()
    {
        // Arrange
        var client = _factory.CreateClient();
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
        var client = _factory.CreateClient();
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
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");
        var authorId = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");

        // Act
        var response = await client.GetAsync($"v1/authors({authorId})");

        // TODO: Refactor this test.
        await Task.Delay(100);

        // Assert
        var author = await response.Content.ReadFromJsonAsync<AuthorViewmodel>();
        author.Should().NotBeNull();
        author.Id.Should().Be(authorId);

        // Verify audit logging.
        _auditContext.Resources.Should().HaveCount(1);
        var auditResource = _auditContext.Resources.First();
        auditResource.ResourceId.Should().Be(author.Id.Value);
    }

    [Fact]
    public async Task Get_AuthorWithBooks_ReturnsAuthorAndBooks()
    {
        // Arrange
        var client = _factory.CreateClient();
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
        var client = _factory.CreateClient();
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
        var client = _factory.CreateClient();
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
        var client = _factory.CreateClient();
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
        var client = _factory.CreateClient();
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
    public async Task Get_AuthorWithBooks_ReturnsAuthorWithBooks()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/Authors?$filter=id eq 8e6a9434-87f5-46b2-a6c3-522dc35d8eef&$expand=books($top=3)");

        // TODO: Refactor this test.
        await Task.Delay(100);

        // Assert
        var odata = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        odata.Should().NotBeNull();

        var authors = odata.Value;
        authors.Should().HaveCount(1);
        authors.Should().NotBeEmpty();
        var author = authors.First();

        author.Should().NotBeNull();

        author.Books.Should().NotBeNull();
        author.Books.Should().HaveCount(3);

        // Verify audit logging.
        _auditContext.Resources.Should().HaveCount(4);
        var authorResource = _auditContext.Resources.First(ar => ar.ResourceType == "Author");
        authorResource.ResourceId.Should().Be(author.Id.Value);

        var bookResources = _auditContext.Resources.Where(ar => ar.ResourceType == "Book");
        bookResources.Should().HaveCount(3);
    }

    [Fact]
    public async Task Get_WithoutParameters_ReturnsOnePageOfAuthors()
    {
        // Arrange
        var client = _factory.CreateClient();
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
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        // Act
        var response = await client.GetAsync($"v1/authors?$top=21");

        // Assert
        //var content = await response.Content.ReadAsStringAsync();
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error.Should().NotBeNull();
        error.Error.Message.Should().Contain("The limit of '20' for Top query has been exceeded");
    }

    [Fact]
    public async Task Put_ValidAuthor_UpdatesAuthor()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        var authorId = "8e6a9434-87f5-46b2-a6c3-522dc35d8eef";
        var newFirstName = "TestFirstName";
        var newLastName = "TestLastName";
        var newBirthday = DateTime.UtcNow - TimeSpan.FromDays(30 * 365);
        using var json = JsonContent.Create(new UpdateAuthorDto(newFirstName, newLastName, newBirthday));

        // Act
        var response = await client.PutAsync($"v1/authors({authorId})", json);

        // Assert request
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var user = new FakeUserService().GetUser();

        // Assert that author is updated.
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        var author = await dbContext.Authors.FindAsync(Guid.Parse(authorId));
        author.FirstName.Should().Be(newFirstName);
        author.LastName.Should().Be(newLastName);
        author.Birthday.Should().Be(newBirthday);
        author.ModifiedBy.Should().Be(user.Id);

        // Assert audit context.
        //var auditContext = _auditContextPublisher.AuditContexts.Single();
        //auditContext.Should().NotBeNull();
        //auditContext.ActorId.Should().Be(user.Id);
        //auditContext.OperationType.Should().Be(OperationType.Update);
        //auditContext.Resources.Should().HaveCount(1);
        //auditContext.Success.Should().BeTrue();
        //var authorResource = auditContext.Resources.First();
        //authorResource.Type.Should().Be(ResourceType.Author);
        //authorResource.Id.Should().Be(Guid.Parse(authorId));
    }

    [Fact]
    public async Task Post_ValidAuthor_AddsAuthor()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept", "application/json;odata.metadata=none");

        var newFirstName = "TestFirstName";
        var newLastName = "TestLastName";
        var newBirthday = DateTime.UtcNow - TimeSpan.FromDays(30 * 365);
        var organizationId = Guid.NewGuid();
        var user = new FakeUserService().GetUser();
        using var json = JsonContent.Create(new AddAuthorCommand(newFirstName, newLastName, newBirthday, organizationId, user));

        // Act
        var response = await client.PostAsync($"v1/authors", json);

        // Assert request
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var authorViewmodel = await response.Content.ReadFromJsonAsync<AuthorViewmodel>();

        // Assert that author is added.
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        var authorDao = await dbContext.Authors.FindAsync(authorViewmodel.Id);
        authorDao.Id.Should().NotBeEmpty();
        authorDao.FirstName.Should().Be(newFirstName);
        authorDao.LastName.Should().Be(newLastName);
        authorDao.Birthday.Should().Be(newBirthday);
        authorDao.ModifiedBy.Should().Be(user.Id);

        authorViewmodel.Id.Should().Be(authorDao.Id);
        authorViewmodel.FirstName.Should().Be(authorDao.FirstName);
        authorViewmodel.LastName.Should().Be(authorDao.LastName);
        //authorViewmodel.Birthday.Should().Be(authorDao.Birthday);
        authorViewmodel.OrganizationId.Should().Be(authorDao.OrganizationId);
        authorViewmodel.Books.Should().BeNull();

        // Assert audit context.
        //var auditContext = _auditContextPublisher.AuditContexts.Single();
        //auditContext.Should().NotBeNull();
        //auditContext.ActorId.Should().Be(user.Id);
        //auditContext.OperationType.Should().Be(OperationType.Create);
        //auditContext.Resources.Should().HaveCount(1);
        ////auditContext.StatusCode.Should().Be(200);
        //auditContext.Success.Should().BeTrue();
        //var authorResource = auditContext.Resources.First();
        //authorResource.Type.Should().Be(ResourceType.Author);
        //authorResource.Id.Should().Be(authorDao.Id);
    }

    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize")]
    public ValueTask DisposeAsync()
    {
        return _factory.DisposeAsync();
    }
}