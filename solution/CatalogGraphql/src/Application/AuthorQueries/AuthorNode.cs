using Domain;
using HotChocolate;
using System.Linq.Expressions;

namespace Application.AuthorQueries;

/// <summary>
/// Represents an author.
/// </summary>
[GraphQLName("Author")]
public class AuthorNode
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
    /// <summary>
    /// Maps an author to an author node.
    /// </summary>
    /// <remarks>Use when expression is not required.</remarks>
    public static AuthorNode ToDto(this Author author)
    {
        return new AuthorNode
        {
            Id = author.Id,
            Birthdate = author.Birthdate,
            FirstName = author.FirstName,
            LastName = author.LastName,
            OrganizationId = author.OrganizationId
        };
    }
    
    /// <summary>
    /// Maps an author to an author node.
    /// </summary>
    /// <remarks>Use when expression is required, for example in EF Core queries.</remarks>
    public static Expression<Func<Author, AuthorNode>> ToNode()
    {
        return author => new AuthorNode
        {
            Id = author.Id,
            Birthdate = author.Birthdate,
            FirstName = author.FirstName,
            LastName = author.LastName,
            OrganizationId = author.OrganizationId
        };
    }
}