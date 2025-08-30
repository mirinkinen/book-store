using Common.Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.OData.Deltas;
using System.Diagnostics.CodeAnalysis;

namespace Cataloging.Domain;

public class Author : Entity
{
    public required DateTime Birthday { get; set; }

    public IReadOnlyList<Book> Books { get; set; } = new List<Book>();

    public required string FirstName { get; set; }

    public required string LastName { get; set; }
    
    public required Guid OrganizationId { get; set; }

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
        if (delta.TryGetChangedPropertyValue(e => e.FirstName, out var firstName))
        {
            FirstName = firstName;
        }

        if (delta.TryGetChangedPropertyValue(e => e.LastName, out var lastName))
        {
            LastName = lastName;
        }

        if (delta.TryGetChangedPropertyValue(e => e.Birthday, out var birthday))
        {
            Birthday = birthday;
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
        var failures = new List<ValidationFailure>();
        
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            failures.Add(new ValidationFailure(nameof(FirstName), $"'{nameof(FirstName)}' cannot be null or whitespace."));
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            failures.Add(new ValidationFailure(nameof(LastName), $"'{nameof(LastName)}' cannot be null or whitespace."));
        }

        if (Birthday == DateTime.MinValue)
        {
            failures.Add(new ValidationFailure(nameof(Birthday), $"'{nameof(Birthday)}' cannot be min value."));
        }

        if (Birthday > DateTime.UtcNow)
        {
            failures.Add(new ValidationFailure(nameof(Birthday), $"'{nameof(Birthday)}' cannot be in future."));
        }

        if (OrganizationId == Guid.Empty)
        {
            failures.Add(new ValidationFailure(nameof(OrganizationId), $"'{nameof(OrganizationId)}' cannot be empty."));
        }

        if (failures.Count != 0)
        {
            throw new ValidationException("Author validation failed.", failures);
        }
    }
}