using Bogus;
using Users.Domain;

namespace Users.Infra.Database.Setup;

public static class MockDataContainer
{
    public static Guid SystemUserId => Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static Guid User1Id => Guid.Parse("8AE0AEFE-76FA-48E5-ABA5-A3F169A9AB24");
    public static Guid User2Id => Guid.Parse("359686D7-5621-4874-BA0D-CD31B1FE96F8");

    public static IEnumerable<User> GetUsers()
    {
        var dateTimeNow = DateTime.UtcNow;
        var dateOnlyNow = DateOnly.FromDateTime(dateTimeNow);
        
        // Return few uses with hard-coded values.
        yield return new User("User1", "User", "user1.user@bookstore.com", dateOnlyNow.AddYears(-25)).SetId(User1Id);
        yield return new User("User2", "User", "user2.user@bookstore.com", dateOnlyNow.AddYears(-40)).SetId(User2Id);

        var userFaker = GetUserFaker(dateTimeNow, dateOnlyNow);
        var users = userFaker.Generate(10_000);

        foreach (var user in users)
        {
            yield return user;
        }
    }

    private static Faker<User> GetUserFaker(DateTime dateTimeNow, DateOnly dateOnlyNow)
    {
        var subscriptionFaker = new Faker<Subscription>()
            .RuleFor(s => s.SubscriptionType, f => f.PickRandom<SubscriptionType>())
            .RuleFor(s => s.StartTime, f => f.Date.Between(dateTimeNow.AddYears(-10), dateTimeNow))
            .RuleFor(s => s.EndTime, (f, u) => u.StartTime.AddYears(1));
        
        var addressFaker = new Faker<Address>()
            .RuleFor(s => s.Country, f => f.Address.Country())
            .RuleFor(s => s.Street, f => f.Address.StreetAddress())
            .RuleFor(s => s.PostalCode, f => f.Address.ZipCode());

        var userFaker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.DateOfBirth, f => f.Date.BetweenDateOnly(dateOnlyNow.AddYears(-80), dateOnlyNow.AddYears(-18)))
            .RuleFor(u => u.Addresses, addressFaker.GenerateBetween(0, 3))
            .RuleFor(u => u.Subscription, subscriptionFaker.Generate());
        return userFaker;
    }
}