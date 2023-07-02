using Cataloging.Domain.Books;
using Cataloging.Domain.SeedWork;
using Common.Domain;

namespace Cataloging.Domain.Authors;

public class Author : Entity
{
    public DateTime Birthday { get; private set; }
    public IReadOnlyList<Book> Books { get; private set; } = new List<Book>();
    public string FirstName { get; private set; }

    public string LastName { get; private set; }
    public Guid OrganizationId { get; private set; }

    public Author(string firstName, string lastName, DateTime birthday, Guid organizationId)
    {
        Update(firstName, lastName, birthday);

        if (organizationId == Guid.Empty)
        {
            throw new DomainRuleException("Empty organization ID not allowed.");
        }

        OrganizationId = organizationId;
    }

    public void Update(string firstName, string lastName, DateTime birthday)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new DomainRuleException($"'{nameof(firstName)}' cannot be null or whitespace.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new DomainRuleException($"'{nameof(lastName)}' cannot be null or whitespace.");
        }

        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;
    }
}