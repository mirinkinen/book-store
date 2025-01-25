namespace Users;

public class Address
{
    public string Country { get; init; }

    public string Street { get; init; }

    public string PostalCode { get; init; }

    public User User { get; set; }
}