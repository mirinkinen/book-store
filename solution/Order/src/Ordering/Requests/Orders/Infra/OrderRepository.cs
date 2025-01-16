using Ordering.Requests.Orders.Domain.Orders;

namespace Ordering.Requests.Orders.Infra;

public class OrderRepository : IOrderRepository
{
    public Task<Order?> GetShoppingCart()
    {
        throw new NotImplementedException();
    }
}