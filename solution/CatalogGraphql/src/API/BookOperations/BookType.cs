using Application.AuthorQueries;
using Application.BookQueries;
using Application.ReviewQueries;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Infra.DataLoaders;

namespace API.BookOperations;

[ObjectType<BookNode>]
public static partial class BookType
{
    
    public static async Task<AuthorNode?> GetAuthorAsync(
        [Parent] BookNode book, 
        QueryContext<AuthorNode> query,
        IAuthorByBookIdDataLoader dataLoader)
    {
        var author = await dataLoader.With(query).LoadAsync(book.Id);
        return author;
    }

    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<ReviewNode>> GetReviewsAsync(
        [Parent] BookNode book,
        PagingArguments pagingArguments,
        QueryContext<ReviewNode> query,
        IReviewsByBookIdDataLoader dataLoader,
        CancellationToken cancellationToken)
    {
        var page = await dataLoader.With(pagingArguments, query).LoadAsync(book.Id, cancellationToken);

        return new PageConnection<ReviewNode>(page ?? Page<ReviewNode>.Empty);
    }
}