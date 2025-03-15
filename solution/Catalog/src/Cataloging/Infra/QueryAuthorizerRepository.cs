using Cataloging.Domain;
using Cataloging.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Cataloging.Infra;

public class QueryAuthorizerRepository : IQueryAuthorizerRepository
{
    private readonly CatalogDbContext _dbContext;

    public QueryAuthorizerRepository(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IQueryable<Author> GetAuthorQuery()
    {
        return _dbContext.Authors.AsNoTracking();
    }

    public IQueryable<Book> GetBookQuery()
    {
        return _dbContext.Books.AsNoTracking();
    }
}