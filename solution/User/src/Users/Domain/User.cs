using Common.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Users.Domain;

public class User : Entity
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public List<Address> Addresses { get; init; } = new List<Address>();

    [SetsRequiredMembers]
    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void AddAddress(Address address)
    {
        address.User = this;
        Addresses.Add(address);
    }
}