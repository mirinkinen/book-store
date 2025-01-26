using Common.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Users.Domain;

public class User : Entity
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

#pragma warning disable CA1002
    public List<Address> Addresses { get; init; } = new List<Address>();
#pragma warning restore CA1002

    /// <summary>
    /// Only for graphql.
    /// </summary>
    private User()
    {
    }
    
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