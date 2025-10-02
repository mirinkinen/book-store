using Application.AuthorQueries;
using Application.BookQueries;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Infra.DataLoaders;

namespace API.AuthorOperations;

[ObjectType<AuthorNode>]
public static partial class AuthorType
{
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<BookNode>> GetBooksAsync(
        [Parent] AuthorNode author, 
        PagingArguments pagingArguments,
        QueryContext<BookNode> query,
        IBooksByAuthorIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        var page = await dataLoader.With(pagingArguments, query).LoadAsync(author.Id, cancellationToken);

        return new PageConnection<BookNode>(page ?? Page<BookNode>.Empty);
    }
    
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<BookNode>> GetCustomBooksAsync(
        [Parent] AuthorNode author, 
        PagingArguments pagingArguments,
        QueryContext<BookNode> query,
        CustomBooksByAuthorIdsDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        var page = await dataLoader.LoadPageAsync(author.Id, pagingArguments, query, cancellationToken);

        return new PageConnection<BookNode>(page);
    }
    
   
}