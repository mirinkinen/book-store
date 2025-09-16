using Domain;

namespace Application.Types;

public static class DtoMappingExtensions
{
    public static AuthorDto ToDto(this Author author) => new()
    {
        Id = author.Id,
        FirstName = author.FirstName,
        LastName = author.LastName,
        Birthdate = author.Birthdate,
        OrganizationId = author.OrganizationId
    };
    
    public static BookDto ToDto(this Book book) => new()
    {
        Id = book.Id,
        AuthorId = book.AuthorId,
        Title = book.Title,
        DatePublished = book.DatePublished,
        Price = book.Price
    };
}