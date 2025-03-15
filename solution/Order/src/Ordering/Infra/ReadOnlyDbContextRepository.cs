using Microsoft.EntityFrameworkCore;
using Ordering.Domain;
using Ordering.Infra.Database;

namespace Ordering.Infra;

public class ReadOnlyDbContextRepository : IReadOnlyDbContextRepository
{
    private readonly OrderDbContext _dbContext;

    public ReadOnlyDbContextRepository(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Order> GetOrderQuery()
    {
        return _dbContext.Orders.AsNoTracking();
    }
}