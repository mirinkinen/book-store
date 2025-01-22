using Ordering.Domain;

namespace Ordering.Infra;

public class OrderRepository : IOrderRepository
{
    public Task<Order?> GetShoppingCart()
    {
        throw new NotImplementedException();
    }
}