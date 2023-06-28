using Cataloging.Application.Services;
using Cataloging.Domain.Authors;
using Cataloging.Domain.Books;
using Cataloging.Domain.SeedWork;
using Cataloging.Infrastructure.Database;

namespace Cataloging.Infrastructure.Queries;

public class QueryAuthorizer : IQueryAuthorizer
{
    private readonly CatalogDbContext _booksDbContext;
    private readonly IUserService _userService;

    public QueryAuthorizer(CatalogDbContext booksDbContext, IUserService userService)
    {
        _booksDbContext = booksDbContext;
        _userService = userService;
    }

    public IQueryable<TEntity> GetAuthorizedEntities<TEntity>() where TEntity : Entity
    {
        var user = _userService.GetUser();

        if (typeof(TEntity) == typeof(Author))
        {
            return (IQueryable<TEntity>)_booksDbContext.Authors.Where(author => user.Organizations.Contains(author.OrganizationId));
        }

        if (typeof(TEntity) == typeof(Book))
        {
            return (IQueryable<TEntity>)_booksDbContext.Books.Where(book => user.Organizations.Contains(book.Author.OrganizationId));
        }

        throw new NotImplementedException();
    }
}