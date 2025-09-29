using Domain;

namespace API.AuthorOperations;

public class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsImplicitly();
    }
}