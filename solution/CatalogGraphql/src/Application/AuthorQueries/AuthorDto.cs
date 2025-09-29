using Application.BookQueries;
using Domain;
using HotChocolate;
using System.Linq.Expressions;

namespace Application.AuthorQueries;

/// <summary>
/// Represents an author.
/// </summary>
public class AuthorDto
{
    /// <summary>
    /// ID of the author.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Birthdate of the author.
    /// </summary>
    public DateOnly Birthdate { get; set; }

    /// <summary>
    /// Books written by the author.
    /// </summary>
    public List<BookDto> Books { get; set; }

    /// <summary>
    /// First name of the author.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Last name of the author. 
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Identifier of the organization associated with the author.
    /// </summary>
    public Guid OrganizationId { get; set; }
}

public static class AuthorExtensions
{
    public static AuthorDto ToDto(this Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Birthdate = author.Birthdate,
            FirstName = author.FirstName,
            LastName = author.LastName,
            OrganizationId = author.OrganizationId
        };
    }

    // Expression-based projection for EF Core
    public static Expression<Func<Author, AuthorDto>> ToDtoExpression()
    {
        return author => new AuthorDto
        {
            Id = author.Id,
            Birthdate = author.Birthdate,
            FirstName = author.FirstName,
            LastName = author.LastName,
            OrganizationId = author.OrganizationId
        };
    }
}