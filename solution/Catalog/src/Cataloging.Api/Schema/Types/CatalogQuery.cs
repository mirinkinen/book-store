using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;
using GraphQL;
using GraphQL.Types;

namespace Cataloging.Api.Schema.Types;

public class CatalogQuery : ObjectGraphType
{
    public CatalogQuery(IQueryAuthorizer queryAuthorizer)
    {
        Field<ListGraphType<AuthorType>>(
            "author",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id"}),
            resolve: context =>
            {
                var authorId = context.GetArgument<Guid>("id");
                return queryAuthorizer.GetAuthorizedEntities<Author>().Where(a => a.Id == authorId);
            });
        
        Field<ListGraphType<AuthorType>>(
            "authors",
            resolve: _ => queryAuthorizer.GetAuthorizedEntities<Author>());
        
        Field<ListGraphType<BookType>>(
            "books",
            resolve: _ => queryAuthorizer.GetAuthorizedEntities<Book>());
    }
}