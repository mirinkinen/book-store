using Common.Application.Auditing;

namespace Cataloging.IntegrationTests.Fakes;

internal class FakeAuditContextPublisher : IAuditContextPublisher
{
    public List<IAuditContext> AuditContexts { get; } = new();

    public Task PublishAuditContext(IAuditContext auditContext)
    {
        AuditContexts.Add(auditContext);

        return Task.CompletedTask;
    }
}