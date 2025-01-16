using Common.Api.Application.Authentication;

namespace Cataloging.Infra.Database.Setup;

internal class SystemUserService : IUserService
{
    public User GetUser()
    {
        return new User(MockDataContainer.SystemUserId, Enumerable.Empty<Guid>());
    }
}