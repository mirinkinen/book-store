using Cataloging.Domain;
using Cataloging.Requests.Books.Domain;
using Common.Domain;
using Microsoft.AspNetCore.OData.Deltas;

namespace Cataloging.Requests.Authors.Domain;

public class Author : Entity
{
    public DateTime Birthday { get; set; }
    public IReadOnlyList<Book> Books { get; set; } = new List<Book>();
    public string FirstName { get; set; }

    public string LastName { get; set; }
    public Guid OrganizationId { get; set; }

    [Obsolete("Only for serialization", true)]
    public Author()
    {
    }

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

    public void Patch(Delta<Author> delta)
    {
        delta.Patch(this);
    }
}