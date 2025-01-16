using Cataloging.Infra.Database;
using Cataloging.Requests.Authors.Domain;

namespace Cataloging.Requests.Authors.Infra;

public class AuthorRepository : IAuthorRepository
{
    private readonly CatalogDbContext _catalogDbContext;

    public AuthorRepository(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public void AddAuthor(Author author)
    {
        _catalogDbContext.Add(author);
    }

    public void Delete(Author author)
    {
        _catalogDbContext.Remove(author);
    }

    public ValueTask<Author?> GetAuthorById(Guid authorId, CancellationToken cancellationToken)
    {
        return _catalogDbContext.FindAsync<Author>(authorId, cancellationToken);
    }

    public Task<int> SaveChangesAsync()
    {
        return _catalogDbContext.SaveChangesAsync();
    }
}