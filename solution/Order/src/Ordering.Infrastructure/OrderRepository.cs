using Ordering.Domain.Orders;

namespace Ordering.Infrastructure;

public class OrderRepository : IOrderRepository
{
    public Task<Order?> GetShoppingCart()
    {
        throw new NotImplementedException();
    }
}