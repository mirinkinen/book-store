using Cataloging.Domain;
using Cataloging.Requests.Books.Domain;
using Common.Domain;
using Microsoft.AspNetCore.OData.Deltas;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Requests.Authors.Domain;

public class Author : Entity
{
    [Required]
    [DataType(DataType.Date)]
    public required DateTime? Birthday { get; set; }

    public IReadOnlyList<Book> Books { get; set; } = new List<Book>();

    [Required]
    [StringLength(32)]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(32)]
    public required string LastName { get; set; }

    [Required]
    public Guid OrganizationId { get; set; }

    [Obsolete("Only for serialization", true)]
    public Author()
    {
    }

    [SetsRequiredMembers]
    public Author(string firstName, string lastName, DateTime? birthday, Guid organizationId)
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

    public void Update(string firstName, string lastName, DateTime? birthday)
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

        if (Birthday is null)
        {
            throw new DomainRuleException($"'{nameof(Birthday)}' cannot be null.");
        }
        
        if (Birthday == DateTime.MinValue)
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