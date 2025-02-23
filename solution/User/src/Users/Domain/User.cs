using Common.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Users.Domain;

public class User : Entity
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public required DateOnly DateOfBirth { get; set; }

    public Subscription Subscription { get; set; }

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
    public User(string firstName, string lastName, string email, DateOnly dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DateOfBirth = dateOfBirth;
    }

    public void AddAddress(Address address)
    {
        address.User = this;
        Addresses.Add(address);
    }
}