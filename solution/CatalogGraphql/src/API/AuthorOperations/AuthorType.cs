using Domain;

namespace API.AuthorOperations;

public class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        base.Configure(descriptor);

        descriptor.BindFieldsExplicitly();
        descriptor.Field(b => b.Id);
        descriptor.Field(b => b.FirstName);
        descriptor.Field(b => b.LastName);
        descriptor.Field(b => b.Birthdate);
        descriptor.Field(b => b.OrganizationId);
        descriptor.Field(b => b.Books);
    }
}