namespace Users.Infra.Database.Setup;

public static class DataSeeder
{
    public static async Task SeedDataAsync(UserDbContext catalogDbContext)
    {
        ArgumentNullException.ThrowIfNull(catalogDbContext);

        var authors = MockDataContainer.GetUsers().ToList();

        // If not already seeded.
        if (!catalogDbContext.Users.Any())
        {
            await catalogDbContext.AddRangeAsync(authors);
            await catalogDbContext.SaveChangesAsync();
        }

        // If not already seeded.
        if (!catalogDbContext.Addresses.Any())
        {
            var books = MockDataContainer.GetAddresses(authors);
            await catalogDbContext.AddRangeAsync(books);
            await catalogDbContext.SaveChangesAsync();
        }
    }
}