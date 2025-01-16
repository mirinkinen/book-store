using Cataloging.Application;
using Cataloging.Domain;
using Cataloging.Infra.Database;
using Cataloging.Requests.Authors.Domain;
using Cataloging.Requests.Books.Domain;
using Common.Application.Authentication;

namespace Cataloging.Infra.Queries;

public class QueryAuthorizer : IQueryAuthorizer
{
    private readonly CatalogDbContext _catalogDbContext;
    private readonly IUserService _userService;

    public QueryAuthorizer(CatalogDbContext catalogDbContext, IUserService userService)
    {
        _catalogDbContext = catalogDbContext;
        _userService = userService;
    }

    public IQueryable<TEntity> GetAuthorizedEntities<TEntity>() where TEntity : Entity
    {
        var user = _userService.GetUser();

        if (typeof(TEntity) == typeof(Author))
        {
            return (IQueryable<TEntity>)_catalogDbContext.Authors.Where(author => user.Organizations.Contains(author.OrganizationId));
        }

        if (typeof(TEntity) == typeof(Book))
        {
            return (IQueryable<TEntity>)_catalogDbContext.Books.Where(book => user.Organizations.Contains(book.Author.OrganizationId));
        }

        throw new NotImplementedException();
    }
}