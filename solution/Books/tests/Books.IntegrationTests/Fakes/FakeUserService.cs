using Books.Application;
using Books.Application.Services;
using Books.MockDataSeeder;

namespace Books.IntegrationTests.Fakes;

internal class FakeUserService : IUserService
{
    public User GetUser()
    {
        return new User(
            Guid.Parse("DC5230E4-E4DC-4DAA-A325-71839CD91F54"),
            new[] { MockDataContainer.AuthorizedOrganization1, MockDataContainer.AuthorizedOrganization2 });
    }
}