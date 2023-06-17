using Books.Api.Domain.Authors;
using Books.Api.Domain.Books;
using Books.Api.Domain.SeedWork;
using Books.Api.Infrastructure.Database;

namespace Books.Api.Application;

public class QueryAuthorizer
{
    private readonly BooksDbContext _booksDbContext;
    private readonly UserService _userService;

    public QueryAuthorizer(BooksDbContext booksDbContext, UserService userService)
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