using HotChocolate.Types;

namespace Application.Types;

[ObjectType("Book")]
public record BookDto(
    Guid Id,
    Guid AuthorId,
    string Title,
    DateOnly DatePublished,
    decimal Price
);