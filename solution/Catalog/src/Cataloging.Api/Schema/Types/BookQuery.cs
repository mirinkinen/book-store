using Cataloging.Application.Services;
using Cataloging.Domain.Books;
using GraphQL.Types;

namespace Cataloging.Api.Schema.Types;

public class BookQuery : ObjectGraphType
{
    public BookQuery(IQueryAuthorizer queryAuthorizer)
    {
        Field<ListGraphType<BookType>>(
            "books",
            resolve: _ => queryAuthorizer.GetAuthorizedEntities<Book>());
    }
}