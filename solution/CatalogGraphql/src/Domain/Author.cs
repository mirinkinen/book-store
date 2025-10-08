using Common.Domain;
using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Domain;

public class Author : Entity
{
    public required DateOnly Birthdate { get; set; }

    public List<Book> Books { get; set; } = new();

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required Guid OrganizationId { get; set; }

    [Obsolete("Only for serialization", true)]
    public Author()
    {
    }

    [SetsRequiredMembers]
    public Author(string firstName, string lastName, DateOnly birthdate, Guid organizationId)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;
        OrganizationId = organizationId;

        Validate();
    }

    public void Update(string firstName, string lastName, DateOnly birthdate)
    {
        FirstName = firstName;
        LastName = lastName;
        Birthdate = birthdate;

        Validate();
    }

    public void AddBook(Book book)
    {
        Books.Add(book);
    }

    public Book RemoveBook(Guid bookId)
    {
        var book = Books.FirstOrDefault(b => b.Id == bookId);
        if (book == null)
        {
            throw new KeyNotFoundException($"Book with ID '{bookId}' not found for this author.");
        }

        Books.Remove(book);
        return book;
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

        if (Birthdate == DateOnly.MinValue)
        {
            failures.Add(new ValidationFailure(nameof(Birthdate), $"'{nameof(Birthdate)}' cannot be min value."));
        }

        if (Birthdate.ToDateTime(TimeOnly.MinValue) > DateTime.UtcNow)
        {
            failures.Add(new ValidationFailure(nameof(Birthdate), $"'{nameof(Birthdate)}' cannot be in future."));
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