using Common.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Users.Domain;

public class Address : Entity
{
    public required string Country { get; set; }

    public required string Street { get; set; }

    public required string PostalCode { get; set; }

    public required Guid UserId { get; set; }

    public User User { get; set; }

    public Address()
    {
    }

    [SetsRequiredMembers]
    public Address(Guid userId, string country, string street, string postalCode)
    {
        UserId = userId;
        Country = country;
        Street = street;
        PostalCode = postalCode;
    }
}