using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Common.Application.Auditing;
using FluentAssertions;
using Wolverine.Tracking;
using Xunit.Abstractions;

namespace Cataloging.IntegrationTests.Authors;

[Trait("Category", "Author")]
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable",
    Justification = "Disposed via IAsyncLifetime")]
public sealed class AuthorIntegrationTests2 : IntegrationContext
{
    private readonly AppFixture _app;
    private readonly ITestOutputHelper _output;

    public AuthorIntegrationTests2(AppFixture app, ITestOutputHelper output) : base(app)
    {
        _app = app;
        _output = output;
    }

    [Fact]
    public async Task Get_Top3_Returns3Authors_Host()
    {
        // Act
        var (tracked, response) = await TrackedHttpCall(scenario => { scenario.Get.Url("/v1/authors?$top=3"); });

        // Assert
        var json = await response.ReadAsTextAsync();
        var odata = JsonSerializer.Deserialize<ValueResponse<AuthorViewmodel>>(json);

        odata.Should().NotBeNull();
        odata.Value.Should().HaveCount(3);

        await Task.Delay(1000);
    }

    [Fact]
    public async Task Get_Audit()
    {
        // Act
        var auditLogEvent = new AuditLogEvent(Guid.NewGuid(), OperationType.Read, new AuditLogResource[]
        {
            new(Guid.NewGuid(), "Author"),
            new(Guid.NewGuid(), "Author")
        });
        var session = await _app.Host!.InvokeMessageAndWaitAsync(auditLogEvent);

        // Assert
    }
}