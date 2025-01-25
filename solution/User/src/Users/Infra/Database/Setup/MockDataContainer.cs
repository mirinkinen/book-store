using Users.Domain;

namespace Users.Infra.Database.Setup;

public static class MockDataContainer
{
    public static Guid SystemUserId => Guid.Parse("11111111-1111-1111-1111-111111111111");

    private static readonly Random _randomizer = new(Guid.NewGuid().GetHashCode());
    public static Guid AuthorizedOrganization1 => Guid.Parse("5D8E6753-1479-408E-BB3D-CB3A02BE486C");
    public static Guid AuthorizedOrganization2 => Guid.Parse("284F633F-2D13-4F4D-8E37-1EE5C9F6B140");

    public static Guid User1Id => Guid.Parse("8AE0AEFE-76FA-48E5-ABA5-A3F169A9AB24");
    public static Guid User2Id => Guid.Parse("359686D7-5621-4874-BA0D-CD31B1FE96F8");

    public static IEnumerable<User> GetUsers()
    {
        var unauthorizedOrganization = Guid.Parse("A34B1695-DB25-4AFF-A717-3C47EC7E89F4");

        yield return new User("User1", "User").SetId(User1Id);
        yield return new User("User2", "User").SetId(User2Id);
    }

    public static IEnumerable<Address> GetAddresses(IEnumerable<User> users)
    {
        var usersList = users.ToList();

        var addresses = new List<Address>();

        // Add books with known IDs for easier testing.
        var address = new Address().SetId(Guid.Parse("8E979C2D-164F-4EDA-B7E3-681E3A7FDE17"));

        addresses.AddRange(new[]
        {
            address,
        });
        
        return addresses;
    }
}