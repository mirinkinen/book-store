using Application.BookQueries;
using Domain;
using HotChocolate;

namespace Application.AuthorQueries;

[GraphQLName("Author")]
public class AuthorDto
{
    public Guid Id { get; set; }
    
    public DateOnly Birthdate { get; set; }

    public List<BookDto> Books { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

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
}