using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Alba;
using Cataloging.Api.Authors;
using Cataloging.Application.Requests.Authors.AddAuthor;
using Cataloging.Application.Requests.Authors.GetAuthors;
using Cataloging.Infrastructure.Database;
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
    private readonly AppFixture _app;
    private readonly JsonSerializerOptions _serializerOptions;

    public AuthorIntegrationTests(AppFixture app) : base(app)
    {
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        _app = app;
    }

    [Fact]
    public async Task Get_Top3_Returns3Authors()
    {
        // Act
        IScenarioResult? result;
        ValueResponse<AuthorViewmodel>? response = null;
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario => scenario.Get.Url("/v1/authors?$top=3"));
            response = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(result.Context.Response.Body);
        });

        // Assert response.
        response.Should().NotBeNull();
        response.Value.Should().HaveCount(3);

        // Assert messages.
        Assert.NotNull(tracked.FindSingleTrackedMessageOfType<GetAuthorsQuery>());
        var auditLogEvent = tracked.FindSingleTrackedMessageOfType<AuditLogEvent>();

        auditLogEvent.ActorId.Should().Be(_app.UserService.GetUser().Id);
        auditLogEvent.Resources.Should().HaveCount(3);
        auditLogEvent.Resources.Should()
            .OnlyContain(alr => alr.ResourceType == "Author" && alr.ResourceId != Guid.Empty);
    }

    [Fact]
    public async Task Get_Select3Properties_Returns3Properties()
    {
        // Arrange
        IScenarioResult? result;
        ValueResponse<AuthorViewmodel>? response = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(
                scenario => scenario.Get.Url("/v1/authors?$top=3&$select=id,firstname,lastname"));
            response = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(result.Context.Response.Body);
        });

        // Assert
        response.Should().NotBeNull();
        var authors = response.Value;
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
        IScenarioResult? result;
        ValueResponse<AuthorViewmodel>? response = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario => scenario.Get.Url("/v1/authors?$filter=contains(firstname,'n')"));
            response = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(result.Context.Response.Body);
        });

        // Assert
        response.Should().NotBeNull();
        var authors = response.Value;
        authors.Should().NotBeEmpty();

        authors.Should().OnlyContain(author => author.FirstName.ContainsIgnoreCase("n"));
    }

    [Fact]
    public async Task Get_AuthorById_ReturnsAuthor()
    {
        // Arrange
        IScenarioResult? result;
        AuthorViewmodel? author = null;
        var authorId = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario => scenario.Get.Url($"/v1/authors({authorId})"));
            author = JsonSerializer.Deserialize<AuthorViewmodel>(result.Context.Response.Body);
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
        IScenarioResult? result;
        AuthorViewmodel? author = null;
        var authorId = Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario => scenario.Get.Url($"/v1/authors({authorId})"));
            author = JsonSerializer.Deserialize<AuthorViewmodel>(result.Context.Response.Body);
        });

        // Assert
        author.Should().NotBeNull();
        author.Id.Should().Be(authorId);
    }

    [Fact]
    public async Task Get_OrderByTitleAscending_ReturnsOrderedAuthors()
    {
        // Arrange
        IScenarioResult? result;
        ValueResponse<AuthorViewmodel>? response = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario => scenario.Get.Url($"/v1/authors?$top=20&orderby=firstname"));
            response = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(result.Context.Response.Body);
        });

        // Assert
        response.Should().NotBeNull();

        var authors = response.Value;
        authors.Should().NotBeEmpty();
        authors.Should().BeInAscendingOrder(author => author.FirstName);
    }

    [Fact]
    public async Task Get_OrderByTitleDescending_ReturnsOrderedAuthors()
    {
        // Arrange
        IScenarioResult? result;
        ValueResponse<AuthorViewmodel>? response = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario => scenario.Get.Url($"/v1/authors?$top=20&orderby=firstname desc"));
            response = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(result.Context.Response.Body);
        });

        response.Should().NotBeNull();

        var authors = response.Value;
        authors.Should().NotBeEmpty();
        authors.Should().BeInDescendingOrder(author => author.FirstName);
    }

    [Fact]
    public async Task Get_Count_ReturnsCount()
    {
        // Arrange
        IScenarioResult? result;
        int? count = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario => scenario.Get.Url($"/v1/authors/$count"));
            count = JsonSerializer.Deserialize<int>(result.Context.Response.Body);
        });

        // Assert
        count.Should().NotBeNull();
        count.Should().NotBe(0);
    }

    [Fact]
    public async Task Get_AuthorsWithBooks_ReturnsAuthorsWithBooks()
    {
        // Arrange
        IScenarioResult? result;
        ValueResponse<AuthorViewmodel>? response = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario =>
                scenario.Get.Url("/v1/Authors?$filter=id eq 8e6a9434-87f5-46b2-a6c3-522dc35d8eef&$expand=Books"));
            response = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(result.Context.Response.Body);
        });

        // Assert
        response.Should().NotBeNull();

        var authors = response.Value;
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
        IScenarioResult? result;
        ValueResponse<AuthorViewmodel>? response = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario =>
                scenario.Get.Url(
                    $"/v1/Authors?$filter=id eq 8e6a9434-87f5-46b2-a6c3-522dc35d8eef&$expand=books($top=3)"));
            response = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(result.Context.Response.Body);
        });

        // Assert
        response.Should().NotBeNull();

        var authors = response.Value;
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
        IScenarioResult? result;
        ValueResponse<AuthorViewmodel>? response = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario => scenario.Get.Url($"/v1/authors"));
            response = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(result.Context.Response.Body);
        });

        // Assert
        response.Should().NotBeNull();

        var authors = response.Value;
        authors.Should().HaveCountLessThanOrEqualTo(20);
    }

    [Fact]
    public async Task Get_TopTooBig_Fails()
    {
        // Arrange
        IScenarioResult? result;
        ErrorResponse? response = null;

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            result = await Host.Scenario(scenario =>
            {
                scenario.Get.Url($"/v1/authors?$top=21");
                scenario.StatusCodeShouldBe(400);
            });

            response = JsonSerializer.Deserialize<ErrorResponse>(result.Context.Response.Body, _serializerOptions);
        });

        // Assert
        response.Should().NotBeNull();
        response.Error.Message.Should().Contain("The limit of '20' for Top query has been exceeded");
    }

    [Fact]
    public async Task Put_ValidAuthor_UpdatesAuthor()
    {
        // Arrange
        IScenarioResult? result = null;
        HttpResponseMessage? response = null;

        var authorId = "8e6a9434-87f5-46b2-a6c3-522dc35d8eef";
        var newFirstName = "TestFirstName";
        var newLastName = "TestLastName";
        var newBirthday = DateTime.UtcNow - TimeSpan.FromDays(30 * 365);
        var command = new UpdateAuthorDto(newFirstName, newLastName, newBirthday);

        // Act
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            var client = Host.Server.CreateClient();
            response = await client.PutAsJsonAsync($"v1/authors({authorId})", command);
        });

        // Assert request
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var user = new FakeUserService().GetUser();

        // Assert that author is updated.
        using var scope = _app.Host.Services.CreateScope();
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
        var user = new FakeUserService().GetUser();
        var command = new AddAuthorCommand(newFirstName, newLastName, newBirthday, organizationId, user);

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
        using var scope = _app.Host.Services.CreateScope();
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
        createAuthorAuditLogEvent.Resources.Should().HaveCount(1);
        var authorResource = createAuthorAuditLogEvent.Resources.First();
        authorResource.ResourceType.Should().Be("Author");
        authorResource.ResourceId.Should().Be(authorDao.Id);
    }
}