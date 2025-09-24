using Domain;

namespace API.Types;

public class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(p => p.Id);
        descriptor.Field(p => p.Birthdate);
        descriptor.Field(p => p.Books);
        descriptor.Field(p => p.FirstName);
        descriptor.Field(p => p.LastName);
        descriptor.Field(p => p.OrganizationId);
    }
}