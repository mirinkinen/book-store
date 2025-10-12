using Application.Services;
using Common.Domain;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Repositories;

public abstract class ReadRepository<TEntity, TNode> : IReadRepository<TEntity, TNode>
    where TEntity : Entity
    where TNode : class
{
    protected IDbContextFactory<CatalogDbContext> DbContextFactory { get; }
    private readonly Lazy<Func<TEntity, TNode>> _compiledProjection;

    protected ReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory)
    {
        DbContextFactory = dbContextFactory;
        _compiledProjection = new Lazy<Func<TEntity, TNode>>(() => GetProjection().Compile());
    }

    protected abstract Expression<Func<TEntity, TNode>> GetProjection();
    protected abstract Func<SortDefinition<TNode>, SortDefinition<TNode>> GetDefaultOrder();

    public virtual async Task<TNode?> GetFirstOrDefaultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity == null ? null : _compiledProjection.Value(entity);
    }

    public virtual async ValueTask<Page<TNode>> GetPage(PagingArguments pagingArguments, QueryContext<TNode> queryContext,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Set<TEntity>()
            .Select(GetProjection())
            .With(queryContext, GetDefaultOrder())
            .ToPageAsync(pagingArguments, cancellationToken);
    }
}