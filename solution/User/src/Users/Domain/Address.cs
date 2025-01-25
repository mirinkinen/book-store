using Common.Domain;

namespace Users.Domain;

public class Address : Entity
{
    public string Country { get; init; }

    public string Street { get; init; }

    public string PostalCode { get; init; }

    public Guid UserId { get; set; }
    
    public User User { get; set; }
}