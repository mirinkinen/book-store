using Cataloging.Application;
using Cataloging.Domain;
using Cataloging.Infra.Database;
using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Infra;

public class QueryAuthorizer : IQueryAuthorizer
{
    private readonly CatalogDbContext _catalogDbContext;

    public QueryAuthorizer(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public Task<IQueryable<TEntity>> GetAuthorizedEntities<TEntity>(User user) where TEntity : Entity
    {
        if (typeof(TEntity) == typeof(Author))
        {
            var query = (IQueryable<TEntity>)_catalogDbContext.Authors.Where(author => user.Organizations.Contains(author.OrganizationId));
            return Task.FromResult(query);
        }

        if (typeof(TEntity) == typeof(Book))
        {
            var query = (IQueryable<TEntity>)_catalogDbContext.Books.Where(book => user.Organizations.Contains(book.Author.OrganizationId));
            return Task.FromResult(query);
        }

        throw new NotImplementedException();
    }
}