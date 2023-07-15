using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Alba;
using Cataloging.Application.Requests.Authors.GetAuthors;
using Common.Application.Auditing;
using FluentAssertions;
using Wolverine.Tracking;

namespace Cataloging.IntegrationTests.Authors;

[Trait("Category", "Author")]
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable",
    Justification = "Disposed via IAsyncLifetime")]
public sealed class AuthorIntegrationTests2 : IntegrationContext
{
    private readonly AppFixture _app;

    public AuthorIntegrationTests2(AppFixture app) : base(app)
    {
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
            // Deserialization of the response is here is important.
            // If the response is not fully waited, tracking ends prematurely
            // and all messages are not tracked.
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
}