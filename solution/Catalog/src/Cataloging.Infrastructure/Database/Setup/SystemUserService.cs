using Common.Application.Authentication;

namespace Cataloging.Infrastructure.Database.Setup;

internal class SystemUserService : IUserService
{
    public User GetUser()
    {
        return new User(MockDataContainer.SystemUserId, Enumerable.Empty<Guid>());
    }
}