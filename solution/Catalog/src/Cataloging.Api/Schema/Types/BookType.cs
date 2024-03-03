using Cataloging.Domain.Books;
using GraphQL.Types;

namespace Cataloging.Api.Schema.Types;

public class BookType : ObjectGraphType<Book>
{
    public BookType()
    {
        Field(b => b.Id).Description("The ID of the book");
        Field(b => b.Price).Description("The price of the book");
        Field(b => b.Title).Description("The title of the book");
        Field(b => b.DatePublished).Description("The date when the book was published");
        Field(b => b.AuthorId).Description("The ID of the author of the book");
    }
}