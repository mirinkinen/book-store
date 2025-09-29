using Domain;

namespace API.BookOperations;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsImplicitly();

    }
}