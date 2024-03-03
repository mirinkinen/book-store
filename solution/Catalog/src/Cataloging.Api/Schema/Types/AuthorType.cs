using Cataloging.Domain.Authors;
using GraphQL.Types;

namespace Cataloging.Api.Schema.Types;

public class AuthorType : ObjectGraphType<Author>
{
    public AuthorType()
    {
        Field(b => b.Id).Description("The ID of the author");
        Field(b => b.FirstName).Description("The firstname of the author");
        Field(b => b.LastName).Description("The lastname of the author");
        Field(b => b.Birthday).Description("The birthday of the author");
    }
}