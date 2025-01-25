using Common.Application.Authentication;

namespace Users.Infra.Database.Setup;

internal class SystemUserService : IUserService
{
    public Task<User> GetUser()
    {
        return Task.FromResult(new User(MockDataContainer.SystemUserId, Enumerable.Empty<Guid>()));
    }
}