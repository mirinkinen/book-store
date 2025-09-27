using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ReadRepository
{
    protected readonly IDbContextFactory<CatalogDbContext> DbContextFactory;

    public ReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory)
    {
        DbContextFactory = dbContextFactory;
    }
}