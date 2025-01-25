using Users.Domain;

namespace Users.Infra.Database.Setup;

public static class MockDataContainer
{
    public static Guid SystemUserId => Guid.Parse("11111111-1111-1111-1111-111111111111");

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
        var userList = users.ToList();
        
        // Add books with known IDs for easier testing.
        yield return new Address(userList[0], "Finland", "Kuusitie", "12345") {}.SetId(Guid.Parse
            ("8E979C2D-164F-4EDA-B7E3-681E3A7FDE17"));
        yield return new Address(userList[1], "Finland", "Tammitie", "23456") {}.SetId(Guid.Parse
            ("C95674A2-F4AE-4346-B4F9-3DB8E6A608AC"));
    }
}