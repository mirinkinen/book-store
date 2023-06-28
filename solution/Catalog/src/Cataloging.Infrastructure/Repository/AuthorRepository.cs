using Cataloging.Domain.Authors;
using Cataloging.Infrastructure.Database;

namespace Cataloging.Infrastructure.Repository;

internal class AuthorRepository : IAuthorRepository
{
    private readonly CatalogDbContext _booksDbContext;

    public AuthorRepository(CatalogDbContext booksDbContext)
    {
        _booksDbContext = booksDbContext;
    }

    public void AddAuthor(Author author)
    {
        _booksDbContext.Add(author);
    }

    public void Delete(Author author)
    {
        _booksDbContext.Remove(author);
    }

    public ValueTask<Author?> GetAuthorById(Guid authorId, CancellationToken cancellationToken)
    {
        return _booksDbContext.FindAsync<Author>(authorId, cancellationToken);
    }

    public Task<int> SaveChangesAsync()
    {
        return _booksDbContext.SaveChangesAsync();
    }
}