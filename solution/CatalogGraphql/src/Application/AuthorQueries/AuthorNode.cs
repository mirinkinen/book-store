using Domain.Authors;
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
    private static readonly Lazy<Func<Author, AuthorNode>> _compiledProjection = new(() => ProjectToNode().Compile());

    /// <summary>
    /// Maps an author to an author node.
    /// </summary>
    public static AuthorNode MapToDto(this Author author)
    {
        return _compiledProjection.Value(author);
    }

    /// <summary>
    /// Projects an author to an author node.
    /// </summary>
    public static Expression<Func<Author, AuthorNode>> ProjectToNode()
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