using Domain.OrderItems;
using HotChocolate;
using System.Linq.Expressions;

namespace Application.OrderItemQueries;

/// <summary>
/// Represents an order item.
/// </summary>
[GraphQLName("OrderItem")]
public class OrderItemNode
{
    /// <summary>
    /// ID of the order item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Quantity of the product ordered.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// ID of the order this item belongs to.
    /// </summary>
    public Guid OrderId { get; set; }
}

public static class OrderItemExtensions
{
    private static readonly Lazy<Func<OrderItem, OrderItemNode>> _compiledProjection = new(() => ProjectToNode().Compile());

    /// <summary>
    /// Maps an order item to an order item node.
    /// </summary>
    public static OrderItemNode MapToDto(this OrderItem orderItem)
    {
        return _compiledProjection.Value(orderItem);
    }

    /// <summary>
    /// Projects an order item to an order item node.
    /// </summary>
    public static Expression<Func<OrderItem, OrderItemNode>> ProjectToNode()
    {
        return orderItem => new OrderItemNode
        {
            Id = orderItem.Id,
            ProductName = orderItem.ProductName,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            OrderId = orderItem.OrderId
        };
    }
}