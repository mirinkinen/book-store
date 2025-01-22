namespace Ordering.Domain;

public interface IOrderRepository
{
    public Task<Order?> GetShoppingCart();
}