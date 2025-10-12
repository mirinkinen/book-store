using Application.BookQueries;
using Domain;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Repositories;

public class BookReadRepository : ReadRepository<Book, BookNode>, IBookReadRepository
{
    public BookReadRepository(IDbContextFactory<CatalogDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    protected override Expression<Func<Book, BookNode>> GetProjection()
    {
        return BookExtensions.ToNode();
    }

    protected override Func<SortDefinition<BookNode>, SortDefinition<BookNode>> GetDefaultOrder()
    {
        return sort => sort.IfEmpty(o => o.AddDescending(t => t.Id));
    }
}