namespace Ordering.Requests.Orders.Domain.Orders;

public interface IOrderRepository
{
    public Task<Order?> GetShoppingCart();
}