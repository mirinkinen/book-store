using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;
using GraphQL.Types;

namespace Cataloging.Api.Schema.Types;

public class CatalogQuery : ObjectGraphType
{
    public CatalogQuery(IQueryAuthorizer queryAuthorizer)
    {
        Field<ListGraphType<BookType>>(
            "books",
            resolve: _ => queryAuthorizer.GetAuthorizedEntities<Book>());
        
        Field<ListGraphType<AuthorType>>(
            "authors",
            resolve: _ => queryAuthorizer.GetAuthorizedEntities<Author>());
    }
}