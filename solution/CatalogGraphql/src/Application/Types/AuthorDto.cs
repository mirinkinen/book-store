using HotChocolate.Types;

namespace Application.Types;

[ObjectType("Author")]
public record AuthorDto(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime Birthdate,
    Guid OrganizationId);