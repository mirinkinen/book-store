using Common.Application.Authentication;

namespace Ordering.Domain;

public class ReadOnlyDbContext : IReadOnlyDbContext
{
    private readonly IReadOnlyDbContextRepository _readOnlyDbContextRepository;

    public ReadOnlyDbContext(IReadOnlyDbContextRepository readOnlyDbContextRepository)
    {
        _readOnlyDbContextRepository = readOnlyDbContextRepository;
    }

    public IQueryable<Order> GetOrders(User user) =>
        _readOnlyDbContextRepository.GetOrderQuery()
            .Where(order => user.Organizations.Contains(order.OrganizationId));
}