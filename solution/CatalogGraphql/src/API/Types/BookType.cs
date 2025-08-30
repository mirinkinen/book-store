using HotChocolate.Types;

namespace API.Types;

/// <summary>
/// GraphQL type definition for Book
/// </summary>
public class BookType : ObjectType<BookDto>
{
    protected override void Configure(IObjectTypeDescriptor<BookDto> descriptor)
    {
        descriptor.Name("Book");
        
        descriptor.Field(b => b.Id).Type<NonNullType<IdType>>();
        descriptor.Field(b => b.Title).Type<NonNullType<StringType>>();
        descriptor.Field(b => b.Published).Type<NonNullType<DateTimeType>>();
        descriptor.Field(b => b.Author).Type<AuthorType>();
        
        // Price is optional in the API, even though it's required in the domain
        descriptor.Field(b => b.Price).Type<DecimalType>();
        
        // Not exposing AuthorId in the GraphQL API
    }
}

/// <summary>
/// DTO for exposing books in the GraphQL API
/// </summary>
public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Published { get; set; }
    public AuthorDto? Author { get; set; }
    public decimal Price { get; set; }
}
