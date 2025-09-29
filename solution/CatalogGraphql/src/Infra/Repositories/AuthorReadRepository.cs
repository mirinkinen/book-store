using Application.AuthorQueries;
using Domain;
using GreenDonut.Data;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AuthorReadRepository : ReadRepository, IAuthorReadRepository
{
    public AuthorReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<AuthorDto?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.Set<Author>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity?.ToDto();
    }

    public async ValueTask<Page<AuthorDto>> With(PagingArguments pagingArguments, QueryContext<AuthorDto> queryContext,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Set<Author>()
            .Select(AuthorExtensions.ToDtoExpression())
            .With(queryContext, DefaultOrder)
            .ToPageAsync(pagingArguments, cancellationToken);
    }

    private static SortDefinition<AuthorDto> DefaultOrder(SortDefinition<AuthorDto> sort)
        => sort.IfEmpty(o => o.AddDescending(t => t.Id));
}