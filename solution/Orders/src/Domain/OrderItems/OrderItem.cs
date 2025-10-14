using Common.Domain;
using Domain.Orders;

namespace Domain.OrderItems;

public class OrderItem : Entity
{
    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public Order Order { get; set; }

    public Guid OrderId { get; set; }

    [Obsolete("Only for serialization", true)]
    public OrderItem()
    {
    }

    public void SetOrder(Order order)
    {
        Order = order ?? throw new ArgumentNullException(nameof(order));
        OrderId = order.Id;
    }

    public OrderItem(Guid orderId, string productName, int quantity, decimal unitPrice)
    {
        OrderId = orderId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}