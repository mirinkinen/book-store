using Domain;

namespace API.Types.Mapping;

/// <summary>
/// Provides mapping functionality between domain models and API DTOs
/// </summary>
public static class DomainToDtoMapper
{
    /// <summary>
    /// Maps a domain Book to a BookDto for GraphQL API exposure
    /// </summary>
    public static BookDto? ToDto(this Book? book)
    {
        if (book == null)
        {
            return null;
        }

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Published = book.DatePublished,
            Price = book.Price,
            Author = book.Author?.ToDto()
        };
    }

    /// <summary>
    /// Maps a domain Author to an AuthorDto for GraphQL API exposure
    /// </summary>
    public static AuthorDto? ToDto(this Author? author)
    {
        if (author == null)
        {
            return null;
        }

        return new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            BirthDate = author.BirthDate,
            Books = author.Books?.Select(b => b.ToDto()).ToList()
        };
    }
}
