using HotChocolate.Types;

namespace API.Types;

/// <summary>
/// GraphQL type definition for Author
/// </summary>
public class AuthorType : ObjectType<AuthorDto>
{
    protected override void Configure(IObjectTypeDescriptor<AuthorDto> descriptor)
    {
        descriptor.Name("Author");
        
        descriptor.Field(a => a.Id).Type<NonNullType<IdType>>();
        descriptor.Field(a => a.FirstName).Type<NonNullType<StringType>>();
        descriptor.Field(a => a.LastName).Type<NonNullType<StringType>>();
        descriptor.Field(a => a.BirthDate).Type<NonNullType<DateTimeType>>();
        descriptor.Field(a => a.Books).Type<ListType<BookType>>();
    }
}

/// <summary>
/// DTO for exposing authors in the GraphQL API
/// </summary>
public class AuthorDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public List<BookDto>? Books { get; set; }
}
