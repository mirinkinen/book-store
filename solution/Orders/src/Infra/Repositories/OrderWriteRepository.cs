using Domain.Orders;
using Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class OrderWriteRepository : WriteRepository<Order>, IOrderWriteRepository
{
    public OrderWriteRepository(OrdersDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> OrderExistsForCustomer(string customerName, DateOnly orderDate, CancellationToken cancellationToken = default)
    {
        return DbContext.Orders.AnyAsync(o => o.CustomerName == customerName && o.OrderDate == orderDate, cancellationToken);
    }
}