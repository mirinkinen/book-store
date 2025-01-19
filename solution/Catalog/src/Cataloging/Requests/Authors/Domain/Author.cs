using Cataloging.Domain;
using Cataloging.Requests.Books.Domain;
using Common.Domain;
using Microsoft.AspNetCore.OData.Deltas;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Requests.Authors.Domain;

public class Author : Entity
{
    public required DateTime Birthday { get; set; }

    public IReadOnlyList<Book> Books { get; set; } = new List<Book>();

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public Guid OrganizationId { get; set; }

    [Obsolete("Only for serialization", true)]
    public Author()
    {
    }

    [SetsRequiredMembers]
    public Author(string firstName, string lastName, DateTime birthday, Guid organizationId)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;
        OrganizationId = organizationId;

        Validate();
    }

    public void Patch(Delta<Author> delta)
    {
        var updatedProperties = GetUpdatedProperties(delta);

        if (updatedProperties.TryGetValue(nameof(FirstName), out var firstName))
        {
            FirstName = firstName.ToString()!;
        }

        if (updatedProperties.TryGetValue(nameof(LastName), out var lastName))
        {
            LastName = lastName.ToString()!;
        }

        if (updatedProperties.TryGetValue(nameof(Birthday), out var birthday))
        {
            Birthday = (DateTime)birthday;
        }

        Validate();
    }

    public void Update(string firstName, string lastName, DateTime birthday)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthday = birthday;

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            throw new DomainRuleException($"'{nameof(FirstName)}' cannot be null or whitespace.");
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            throw new DomainRuleException($"'{nameof(LastName)}' cannot be null or whitespace.");
        }

        if (Birthday == default)
        {
            throw new DomainRuleException($"'{nameof(Birthday)}' cannot be min value.");
        }

        if (Birthday > DateTime.UtcNow)
        {
            throw new DomainRuleException($"'{nameof(Birthday)}' cannot be in future.");
        }

        if (OrganizationId == Guid.Empty)
        {
            throw new DomainRuleException("Empty organization ID not allowed.");
        }
    }
}