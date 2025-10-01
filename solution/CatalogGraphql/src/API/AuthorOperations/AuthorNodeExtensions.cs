using Application.AuthorQueries;
using Application.BookQueries;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Infra.DataLoaders;

namespace API.AuthorOperations;

[ExtendObjectType<AuthorNode>]
public static class AuthorNodeExtensions
{
    // public static async Task<PageConnection<BookNode>?> GetBooksAsync(
    //         [Parent] AuthorNode author, 
    //         PagingArguments pagingArguments,
    //         CustomBooksByAuthorIdsDataLoader dataLoader)
   
    public static async Task<PageConnection<BookNode>?> GetBooksAsync(
        [Parent] AuthorNode author, 
        PagingArguments pagingArguments,
        IBooksByAuthorIdsDataLoader dataLoader)
    {
        var page = await dataLoader.With(pagingArguments).LoadAsync(author.Id);
        return new PageConnection<BookNode>(page);
    }
}