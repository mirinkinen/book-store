using Users.Domain;
using Users.Infra.Database;

namespace Users.API;

public class Queries
{
    [UsePaging]
    public IQueryable<User> GetUsers(UserDbContext userDbContext)
    {
        return userDbContext.Users;
    }
}