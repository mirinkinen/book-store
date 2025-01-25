using Users.Domain;
using Users.Infra.Database;

namespace Users.API;

public class Query
{
    public IQueryable<User> GetUsers(UserDbContext userDbContext)
    {
        return userDbContext.Users;
    }
}