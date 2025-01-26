using Microsoft.EntityFrameworkCore;

namespace Users.Infra.Database.Setup;

public static class DataRemover
{
    public static async Task RemoveDataAsync(UserDbContext userDbContext)
    {
        ArgumentNullException.ThrowIfNull(userDbContext);

        await userDbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Users.Address");
        await userDbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Users.User");
    }
}