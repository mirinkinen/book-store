using Domain;

namespace Application.AuthorQueries;

/// <summary>
/// Represents an author.
/// </summary>
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
}