using Application.Services;
using Domain.OrderItems;

namespace Application.OrderItemQueries;

public interface IOrderItemReadRepository : IReadRepository<OrderItem, OrderItemNode>
{
}