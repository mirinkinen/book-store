using Common.Application.Authentication;

namespace Ordering.Domain;

public interface IReadOnlyDbContext
{
    IQueryable<Order> GetOrders(User user);
}