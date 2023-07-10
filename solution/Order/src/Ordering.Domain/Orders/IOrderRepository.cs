namespace Ordering.Domain.Orders;

public interface IOrderRepository
{
    public Task<Order?> GetShoppingCart();
}