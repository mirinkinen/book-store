using Common.Domain;

namespace Users.Domain;

public class Address : Entity
{
    public string Country { get; init; }

    public string Street { get; init; }

    public string PostalCode { get; init; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Address()
    {
    }

    public Address(User user, string country, string street, string postalCode)
    {
        Country = country;
        Street = street;
        PostalCode = postalCode;
        User = user;
    }
}