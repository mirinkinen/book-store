using Cataloging.API.Models;
using Cataloging.Application.AddAuthor;
using Cataloging.Application.GetAuthors;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using Cataloging.Infra.Database;
using Cataloging.IntegrationTests.Fakes;
using Common.Application.Auditing;
using FluentAssertions;
using JasperFx.Core;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Tracking;

namespace Cataloging.IntegrationTests.Authors;

[Trait("Category", "Author")]
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable",
    Justification = "Disposed via IAsyncLifetime")]
public sealed class AuthorIntegrationTests : IntegrationContext
{
    public AuthorIntegrationTests(AppFixture app) : base(app)
    {
    }

    [Fact]
    public async Task Get_Top3_Returns3Authors()
    {
        // Arrange
        ValueResponse<AuthorViewmodel>? content = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("/v1/authors?$top=3");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        });

        // Assert content
        content.Should().NotBeNull();
        content.Value.Should().HaveCount(3);

        // Assert messages
        Assert.NotNull(tracked.FindSingleTrackedMessageOfType<GetAuthorsQuery>());
        var auditLogEvent = tracked.FindSingleTrackedMessageOfType<AuditLogEvent>();

        auditLogEvent.ActorId.Should().Be((await UserService.GetUser()).Id);
        auditLogEvent.Resources.Should().HaveCount(3);
        auditLogEvent.Resources.Should()
            .OnlyContain(alr => alr.ResourceType == "Author" && alr.ResourceId != Guid.Empty);
    }

    [Fact]
    public async Task Get_Select3Properties_Returns3Properties()
    {
        // Arrange
        ValueResponse<AuthorViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("/v1/authors?$top=3&$select=id,firstname,lastname");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();
        var authors = content.Value;
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
        ValueResponse<AuthorViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("/v1/authors?$filter=contains(firstname,'n')");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();
        var authors = content.Value;
        authors.Should().NotBeEmpty();

        authors.Should().OnlyContain(author => author.FirstName!.ContainsIgnoreCase("n"));
    }

    [Fact]
    public async Task Get_AuthorById_ReturnsAuthor()
    {
        // Arrange
        AuthorViewmodel? author = null;
        var authorId = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync($"/v1/authors/{authorId}");
            author = await response.Content.ReadFromJsonAsync<AuthorViewmodel>();
        });

        // Assert
        author.Should().NotBeNull();
        author.Id.Should().Be(authorId);

        // Verify audit logging.
        var auditLogEvent = tracked.FindSingleTrackedMessageOfType<AuditLogEvent>();
        auditLogEvent.Resources.Should().HaveCount(1);
        var auditResource = auditLogEvent.Resources.First();
        auditResource.ResourceId.Should().Be(author.Id.Value);
    }

    [Fact]
    public async Task Get_AuthorWithBooks_ReturnsAuthorAndBooks()
    {
        // Arrange
        AuthorViewmodel? author = null;
        var authorId = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync($"/v1/authors/{authorId}");
            author = await response.Content.ReadFromJsonAsync<AuthorViewmodel>();
        });

        // Assert
        author.Should().NotBeNull();
        author.Id.Should().Be(authorId);
    }

    [Fact]
    public async Task Get_OrderByTitleAscending_ReturnsOrderedAuthors()
    {
        // Arrange
        ValueResponse<AuthorViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync($"/v1/authors?$top=20&orderby=firstname");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();

        var authors = content.Value;
        authors.Should().NotBeEmpty();
        authors.Should().BeInAscendingOrder(author => author.FirstName);
    }

    [Fact]
    public async Task Get_OrderByTitleDescending_ReturnsOrderedAuthors()
    {
        // Arrange
        ValueResponse<AuthorViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("/v1/authors?$top=20&orderby=firstname desc");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        });

        content.Should().NotBeNull();

        var authors = content.Value;
        authors.Should().NotBeEmpty();
        authors.Should().BeInDescendingOrder(author => author.FirstName);
    }

    [Fact]
    public async Task Get_Count_ReturnsCount()
    {
        // Arrange
        int? count = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("/v1/authors/$count");
            count = await response.Content.ReadFromJsonAsync<int>();
        });

        // Assert
        count.Should().NotBeNull();
        count.Should().NotBe(0);
    }

    [Fact]
    public async Task Get_AuthorsWithBooks_ReturnsAuthorsWithBooks()
    {
        // Arrange
        ValueResponse<AuthorViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response =
                await client.GetAsync("/v1/Authors?$filter=id eq 8e6a9434-87f5-46b2-a6c3-522dc35d8eef&$expand=Books");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();

        var authors = content.Value;
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
        ValueResponse<AuthorViewmodel>? content = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response =
                await client.GetAsync(
                    "/v1/Authors?$filter=id eq 8e6a9434-87f5-46b2-a6c3-522dc35d8eef&$expand=books($top=3)");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();

        var authors = content.Value;
        authors.Should().HaveCount(1);
        authors.Should().NotBeEmpty();

        var author = authors.First();
        author.Should().NotBeNull();
        author.Books.Should().NotBeNull();
        author.Books.Should().HaveCount(3);

        // Verify audit logging.
        var auditLogEvent = tracked.FindSingleTrackedMessageOfType<AuditLogEvent>();
        auditLogEvent.Resources.Should().HaveCount(4);
        var authorResource = auditLogEvent.Resources.First(ar => ar.ResourceType == "Author");
        authorResource.ResourceId.Should().Be(author.Id.Value);

        var bookResources = auditLogEvent.Resources.Where(ar => ar.ResourceType == "Book");
        bookResources.Should().HaveCount(3);
    }

    [Fact]
    public async Task Get_WithoutParameters_ReturnsOnePageOfAuthors()
    {
        // Arrange
        ValueResponse<AuthorViewmodel>? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("/v1/authors");
            content = await response.Content.ReadFromJsonAsync<ValueResponse<AuthorViewmodel>>();
        });

        // Assert
        content.Should().NotBeNull();

        var authors = content.Value;
        authors.Should().HaveCountLessThanOrEqualTo(20);
    }

    [Fact]
    public async Task Get_TopTooBig_Fails()
    {
        // Arrange
        ErrorResponse? content = null;

        // Act
        var _ = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            var response = await client.GetAsync("/v1/authors?$top=21");
            content = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        });

        // Assert
        content.Should().NotBeNull();
        content.Error.Message.Should().Contain("The limit of '20' for Top query has been exceeded");
    }

    [Fact]
    public async Task Put_ValidAuthor_UpdatesAuthor()
    {
        // Arrange
        var authorId = "8e6a9434-87f5-46b2-a6c3-522dc35d8eef";
        var newFirstName = "TestFirstName";
        var newLastName = "TestLastName";
        var newBirthday = DateTime.UtcNow - TimeSpan.FromDays(30 * 365);
        
        var putAuthorDto = new PutAuthorDtoV1
        {
            FirstName = newFirstName,
            LastName = newLastName,
            Birthday = newBirthday  
        };  
        
        HttpResponseMessage? response = null;
    
        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            response = await client.PutAsJsonAsync($"v1/authors/{authorId}", putAuthorDto);
        });
    
        // Assert request
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    
        var user = await new FakeUserService().GetUser();
    
        // Assert that author is updated.
        using var scope = Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        var authorDao = await dbContext.Authors.FindAsync(Guid.Parse(authorId));
        authorDao.FirstName.Should().Be(newFirstName);
        authorDao.LastName.Should().Be(newLastName);
        authorDao.Birthday.Should().Be(newBirthday);
        authorDao.ModifiedBy.Should().Be(user.Id);
    
        // Assert audit context.
        var createAuthorAuditLogEvent = tracked.FindEnvelopesWithMessageType<AuditLogEvent>()
            .Single(e => e is
            {
                MessageEventType: MessageEventType.Sent,
                Message: AuditLogEvent { OperationType: OperationType.Update }
            })
            .Message as AuditLogEvent;
    
        createAuthorAuditLogEvent.Should().NotBeNull();
        createAuthorAuditLogEvent.Resources.Should().HaveCount(1);
        var authorResource = createAuthorAuditLogEvent.Resources.First();
        authorResource.ResourceType.Should().Be("Author");
        authorResource.ResourceId.Should().Be(authorDao.Id);
    }

    [Fact]
    public async Task Post_ValidAuthor_AddsAuthor()
    {
        // Arrange
        HttpResponseMessage? response = null;

        var newFirstName = "TestFirstName";
        var newLastName = "TestLastName";
        var newBirthday = DateTime.UtcNow - TimeSpan.FromDays(30 * 365);
        var organizationId = Guid.NewGuid();
        var user = await new FakeUserService().GetUser();
        
        var command = new PostAuthorCommand(newFirstName, newLastName, newBirthday, organizationId);

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            response = await client.PostAsJsonAsync("/v1/authors", command);
        });

        // Assert request
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var author = await response.Content.ReadFromJsonAsync<AuthorViewmodel>();

        // Assert that author is added.
        using var scope = Host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        var authorDao = await dbContext.Authors.FindAsync(author.Id);
        authorDao.Id.Should().NotBeEmpty();
        authorDao.FirstName.Should().Be(newFirstName);
        authorDao.LastName.Should().Be(newLastName);
        // authorDao.Birthday.Should().Be(newBirthday);
        authorDao.ModifiedBy.Should().Be(user.Id);

        author.Id.Should().Be(authorDao.Id);
        author.FirstName.Should().Be(authorDao.FirstName);
        author.LastName.Should().Be(authorDao.LastName);
        // author.Birthday.Should().Be(authorDao.Birthday);
        author.OrganizationId.Should().Be(authorDao.OrganizationId);
        author.Books.Should().BeNull();

        // Assert audit context.
        var createAuthorAuditLogEvent = tracked.FindEnvelopesWithMessageType<AuditLogEvent>()
            .Single(e => e is
            {
                MessageEventType: MessageEventType.Sent,
                Message: AuditLogEvent { OperationType: OperationType.Create }
            })
            .Message as AuditLogEvent;

        var readAuthorAuditLogEvent = tracked.FindEnvelopesWithMessageType<AuditLogEvent>()
            .Single(e => e is
            {
                MessageEventType: MessageEventType.Sent,
                Message: AuditLogEvent { OperationType: OperationType.Read }
            })
            .Message as AuditLogEvent;

        createAuthorAuditLogEvent.Should().NotBeNull();
        createAuthorAuditLogEvent.OperationType.Should().Be(OperationType.Create);
        createAuthorAuditLogEvent.Resources.Should().HaveCount(1);
        var authorResource = createAuthorAuditLogEvent.Resources.First();
        authorResource.ResourceType.Should().Be("Author");
        authorResource.ResourceId.Should().Be(authorDao.Id);

        readAuthorAuditLogEvent.Should().NotBeNull();
        readAuthorAuditLogEvent.OperationType.Should().Be(OperationType.Read);
        readAuthorAuditLogEvent.Resources.Should().HaveCount(1);
        authorResource = readAuthorAuditLogEvent.Resources.First();
        authorResource.ResourceType.Should().Be("Author");
        authorResource.ResourceId.Should().Be(authorDao.Id);
    }
}