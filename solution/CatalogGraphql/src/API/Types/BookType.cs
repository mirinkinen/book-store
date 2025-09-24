using Domain;

namespace API.Types;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(p => p.Id);
        descriptor.Field(p => p.AuthorId);
        descriptor.Field(p => p.Title);
        descriptor.Field(p => p.DatePublished);
        descriptor.Field(p => p.Price);
        descriptor.Field(p => p.Author);
    }
}