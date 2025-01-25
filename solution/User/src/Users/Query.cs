namespace Users;

public class Query
{
    public string Hello(string name = "World")
    {
        return "Hello " + name;
    }

    public IEnumerable<User> GetUsers()
    {
        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
        };

        user.AddAddress(new Address
        {
            Country = "USA",
            Street = "123 Elm St",
            User = user
        });

        user.AddAddress(new Address
        {
            Country = "Canada",
            Street = "456 Maple St",
            User = user
        });

        yield return user;
    }
}