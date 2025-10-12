using Domain;
using Domain.Authors;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AuthorWriteRepository : WriteRepository<Author>, IAuthorWriteRepository
{
    public AuthorWriteRepository(CatalogDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> AuthorWithNameExists(string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        return DbContext.Authors.AnyAsync(a => a.FirstName == firstName && a.LastName == lastName, cancellationToken);
    }
}