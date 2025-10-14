using Domain.Orders;
using HotChocolate;
using System.Linq.Expressions;

namespace Application.OrderQueries;

/// <summary>
/// Represents an order.
/// </summary>
[GraphQLName("Order")]
public class OrderNode
{
    /// <summary>
    /// ID of the order.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the customer who placed the order.
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// Date when the order was placed.
    /// </summary>
    public DateOnly OrderDate { get; set; }

    /// <summary>
    /// Total amount of the order.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Identifier of the organization associated with the order.
    /// </summary>
    public Guid OrganizationId { get; set; }
}

public static class OrderExtensions
{
    private static readonly Lazy<Func<Order, OrderNode>> _compiledProjection = new(() => ProjectToNode().Compile());

    /// <summary>
    /// Maps an order to an order node.
    /// </summary>
    public static OrderNode MapToDto(this Order order)
    {
        return _compiledProjection.Value(order);
    }

    /// <summary>
    /// Projects an order to an order node.
    /// </summary>
    public static Expression<Func<Order, OrderNode>> ProjectToNode()
    {
        return order => new OrderNode
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            OrganizationId = order.OrganizationId
        };
    }
}