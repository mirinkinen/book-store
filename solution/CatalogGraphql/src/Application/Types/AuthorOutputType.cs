using HotChocolate.Types;

namespace Application.Types;

[ObjectType("Author")]
public class AuthorOutputType
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime Birthdate { get; set; }
    public required Guid OrganizationId { get; set; }
}