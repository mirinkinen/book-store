namespace Ordering.Domain;

public interface IReadOnlyDbContextRepository
{
    IQueryable<Order> GetOrderQuery();
}