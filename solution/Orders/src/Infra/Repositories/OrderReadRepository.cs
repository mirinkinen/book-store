using Application.OrderQueries;
using Domain.Orders;
using GreenDonut.Data;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Repositories;

public class OrderReadRepository : ReadRepository<Order, OrderNode>, IOrderReadRepository
{
    public OrderReadRepository(IDbContextFactory<OrdersDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    protected override Expression<Func<Order, OrderNode>> GetProjection()
    {
        return OrderExtensions.ProjectToNode();
    }

    protected override Func<SortDefinition<OrderNode>, SortDefinition<OrderNode>> GetDefaultOrder()
    {
        return sort => sort.IfEmpty(o => o.AddDescending(a => a.OrderDate))
            .AddDescending(t => t.Id);
    }
}