using Cataloging.Infra.Database.Setup;
using Common.Application.Authentication;

namespace Cataloging.IntegrationTests.Fakes;

public class FakeUserAccessor : IUserAccessor
{
    public Task<User> GetUser()
    {
        return Task.FromResult(new User(
            Guid.Parse("DC5230E4-E4DC-4DAA-A325-71839CD91F54"),
            new[] { MockDataContainer.AuthorizedOrganization1, MockDataContainer.AuthorizedOrganization2 }));
    }
}