using Domain.OrderItems;
using Infra.Database;

namespace Infra.Repositories;

public class OrderItemWriteRepository : WriteRepository<OrderItem>, IOrderItemWriteRepository
{
    public OrderItemWriteRepository(OrdersDbContext dbContext) : base(dbContext)
    {
    }
}