using Domain;

namespace API.BookOperations;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsExplicitly();
        descriptor.Field(b => b.Id);
        descriptor.Field(b => b.Title);
        descriptor.Field(b => b.Price);
        descriptor.Field(b => b.DatePublished);
        descriptor.Field(b => b.Author);
        descriptor.Field(b => b.AuthorId);
    }
}