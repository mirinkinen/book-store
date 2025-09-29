using Application.AuthorQueries;
using Application.AuthorQueries.GetAuthorById;
using Application.AuthorQueries.GetAuthors;
using Common.Domain;
using Domain;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;
using Infra.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace API.AuthorOperations;

[QueryType]
public static partial class AuthorQueries
{
    private static readonly ActivitySource _activity = new(nameof(AuthorQueries));
    
    [NodeResolver]
    [Error<EntityNotFoundException>]
    public static async Task<AuthorDto> GetAuthorById(Guid id, ISender sender)
    {
        return await sender.Send(new GetAuthorByIdQuery(id));
    }

    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<AuthorDto>> GetAuthors(
        PagingArguments pagingArguments,
        QueryContext<AuthorDto> queryContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var page = await sender.Send(new GetAuthorsQuery(pagingArguments, queryContext), cancellationToken);
        return new PageConnection<AuthorDto>(page);
    }
    
    [UseConnection]
    [UseFiltering]
    [UseSorting]
    public static async Task<PageConnection<Author>> GetAuthorEntities(
        PagingArguments pagingArguments,
        QueryContext<Author> queryContext,
        IDbContextFactory<CatalogDbContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        using var activity = _activity.StartActivity();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var page = await dbContext.Authors
            .Include(a => a.Books)
            .With(queryContext, sort => sort.IfEmpty(o => o.AddDescending(t => t.Id)))
            .ToPageAsync(pagingArguments, cancellationToken);
        
        return new PageConnection<Author>(page);
    }
}