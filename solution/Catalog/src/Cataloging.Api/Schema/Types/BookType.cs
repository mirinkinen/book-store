using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;
using Cataloging.Infrastructure.Database;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace Cataloging.Api.Schema.Types;

public class BookType : ObjectGraphType<Book>
{
    public BookType(IDataLoaderContextAccessor dataLoaderContextAccessor)
    {
        Field(b => b.Id).Description("The ID of the book");
        Field(b => b.Price).Description("The price of the book");
        Field(b => b.Title).Description("The title of the book");
        Field(b => b.DatePublished).Description("The date when the book was published");
        Field(b => b.AuthorId).Description("The ID of the author of the book");

        Field<AuthorType, Author>("author").ResolveAsync(context =>
        {
            var dbContext = context.RequestServices!.GetRequiredService<CatalogDbContext>();

            var loader = dataLoaderContextAccessor.Context!.GetOrAddBatchLoader<Guid, Author>("GetAuthorById",
                async authorIds =>
                {
                    var authors = await dbContext.Authors.Where(a => authorIds.Contains(context.Source.AuthorId))
                        .ToListAsync();

                    return authors.ToDictionary(a => a.Id);
                });

            return loader.LoadAsync(context.Source.AuthorId);
        });
    }
}