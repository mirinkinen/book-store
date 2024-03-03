using Cataloging.Domain.Books;
using Cataloging.Infrastructure.Database;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

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

        Field<AuthorType>("author", resolve: GetAuthor);
    }

    private object? GetAuthor(IResolveFieldContext<Book> context)
    {
        var dbContext = context.RequestServices!.GetRequiredService<CatalogDbContext>();

        return dbContext.Authors.FirstOrDefault(a => a.Id == context.Source.AuthorId);
    }
}