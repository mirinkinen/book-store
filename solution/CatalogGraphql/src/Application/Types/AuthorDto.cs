using Domain;
using HotChocolate.Types;

namespace Application.Types;

[ObjectType("Author")]
public class AuthorDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public Guid OrganizationId { get; set; }
    
    public IEnumerable<BookDto> Books { get; set; }
    
    
}