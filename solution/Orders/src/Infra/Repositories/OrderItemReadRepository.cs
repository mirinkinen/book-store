using Application.OrderItemQueries;
using Domain.OrderItems;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Repositories;

public class OrderItemReadRepository : ReadRepository<OrderItem, OrderItemNode>, IOrderItemReadRepository
{
    public OrderItemReadRepository(IDbContextFactory<OrdersDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    protected override Expression<Func<OrderItem, OrderItemNode>> GetProjection()
    {
        return OrderItemExtensions.ProjectToNode();
    }

    protected override Func<SortDefinition<OrderItemNode>, SortDefinition<OrderItemNode>> GetDefaultOrder()
    {
        return sort => sort.IfEmpty(o => o.AddAscending(a => a.ProductName))
            .AddDescending(t => t.Id);
    }
}