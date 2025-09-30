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

    public async Task<AuthorNode?> FirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.Set<Author>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity?.ToDto();
    }

    public async ValueTask<Page<AuthorNode>> With(PagingArguments pagingArguments, QueryContext<AuthorNode> queryContext,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Set<Author>()
            .Select(a => new AuthorNode
            {
                Id = a.Id,
                Birthdate = a.Birthdate,
                FirstName = a.FirstName,
                LastName = a.LastName,
                OrganizationId = a.OrganizationId
            })
            .With(queryContext, DefaultOrder)
            .ToPageAsync(pagingArguments, cancellationToken);
    }

    private static SortDefinition<AuthorNode> DefaultOrder(SortDefinition<AuthorNode> sort)
        => sort.IfEmpty(o => o.AddDescending(t => t.Id));
}