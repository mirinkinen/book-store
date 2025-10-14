using Application.Services;
using Domain.Orders;

namespace Application.OrderQueries;

public interface IOrderReadRepository : IReadRepository<Order, OrderNode>
{
}