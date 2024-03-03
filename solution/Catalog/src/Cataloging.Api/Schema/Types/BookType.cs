using Cataloging.Domain.Books;
using GraphQL.Types;

namespace Cataloging.Api.Schema.Types;

public class BookType : ObjectGraphType<Book>
{
    public BookType()
    {
        Field(b => b.Id);
        Field(b => b.Price);
        Field(b => b.Title);
    }
}
