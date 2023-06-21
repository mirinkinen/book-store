using Books.Domain.Books;
using Books.Domain.SeedWork;
using System.Collections.ObjectModel;

namespace Books.Domain.Authors;

public class Author : Entity
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public DateTime Birthday { get; private set; }

    public ReadOnlyCollection<Book> Books { get; private set; } = new List<Book>().AsReadOnly();

    public Guid OrganizationId { get; private set; }

    public Author(string firstName, string lastName, DateTime birthday, Guid organizationId)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new DomainRuleException($"'{nameof(firstName)}' cannot be null or whitespace.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new DomainRuleException($"'{nameof(lastName)}' cannot be null or whitespace.");
        }

        if (organizationId == Guid.Empty)
        {
            throw new DomainRuleException("Empty organization ID not allowed.");
        }

        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;
        OrganizationId = organizationId;
    }
}