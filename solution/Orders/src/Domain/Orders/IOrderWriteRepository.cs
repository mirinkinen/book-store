namespace Domain.Orders;

/// <summary>
/// Repository interface for Order entity
/// </summary>
public interface IOrderWriteRepository : IWriteRepository<Order>
{
    Task<bool> OrderExistsForCustomer(string customerName, DateOnly orderDate, CancellationToken cancellationToken = default);
}
