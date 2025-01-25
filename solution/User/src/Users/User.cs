namespace Users;

public class User
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public List<Address> Addresses { get; init; } = new List<Address>();

    public void AddAddress(Address address)
    {
        address.User = this;
        Addresses.Add(address);
    }
}