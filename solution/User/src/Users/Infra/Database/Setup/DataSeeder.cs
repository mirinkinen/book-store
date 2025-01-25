namespace Users.Infra.Database.Setup;

public static class DataSeeder
{
    public static async Task SeedDataAsync(UserDbContext userDbContext)
    {
        ArgumentNullException.ThrowIfNull(userDbContext);

        var users = MockDataContainer.GetUsers().ToList();

        // If not already seeded.
        if (!userDbContext.Users.Any())
        {
            await userDbContext.AddRangeAsync(users);
            await userDbContext.SaveChangesAsync();
        }

        // If not already seeded.
        if (!userDbContext.Addresses.Any())
        {
            var addresses = MockDataContainer.GetAddresses(users);
            await userDbContext.AddRangeAsync(addresses);
            await userDbContext.SaveChangesAsync();
        }
    }
}