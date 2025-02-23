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

        for (var i = 0; i < 10_000; i++)
        {
            yield return GetUser(dateTimeNow, dateOnlyNow);
        }
    }

    private static User GetUser(DateTime dateTimeNow, DateOnly dateOnlyNow)
    {
        var userFaker = new Faker<User>()
            .CustomInstantiator(f => new User(
                f.Name.FirstName(),
                f.Name.LastName(),
                f.Internet.Email(),
                f.Date.BetweenDateOnly(dateOnlyNow.AddYears(-80), dateOnlyNow.AddYears(-18))));

        var user = userFaker.Generate();

        var addressFaker = new Faker<Address>()
            .CustomInstantiator(f => new Address(
                user.Id,
                f.Address.Country(),
                f.Address.StreetAddress(),
                f.Address.ZipCode()));

        foreach (var address in addressFaker.GenerateBetween(0, 3))
        {
            user.AddAddress(address);
        }

        var subscriptionFaker = new Faker<Subscription>()
            .CustomInstantiator(f =>
                new Subscription(user.Id, f.PickRandom<SubscriptionType>(), f.Date.Between(dateTimeNow.AddYears(-10), dateTimeNow)));

        user.SetSubscription(subscriptionFaker.Generate());

        return user;
    }
}