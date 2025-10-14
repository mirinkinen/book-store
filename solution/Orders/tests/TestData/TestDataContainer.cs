using Common.Domain;
using Common.Infra;
using Domain.Orders;
using Domain.OrderItems;

namespace TestData;

public static class TestDataContainer
{
    // System user ID for test data.
    public static Guid SystemUserId => Guid.Parse("00000000-0000-0000-0000-000000000001");

    // Organization IDs for test data.
    public static Guid AuthorizedOrganization1 => Guid.Parse("10000000-0000-0000-0000-000000000001");
    public static Guid AuthorizedOrganization2 => Guid.Parse("10000000-0000-0000-0000-000000000002");

    // Order IDs for test data.
    public static Guid Order1Id => Guid.Parse("8E6A9434-87F5-46B2-A6C3-522DC35D8EEF");
    public static Guid Order2Id => Guid.Parse("7D5B8323-76E4-35A1-B5B2-411CB24C7DEE");
    public static Guid Order3Id => Guid.Parse("6C4A7212-65D3-24F0-A4A1-300BA13B6CDD");

    // Order Item IDs for test data.
    public static Guid OrderItem1Id => Guid.Parse("A125C5BD-4F8E-4794-9C36-76E401FB4F24");
    public static Guid OrderItem2Id => Guid.Parse("B234D6CE-5E9F-5805-AD47-87F512FC5E35");
    public static Guid OrderItem3Id => Guid.Parse("C345E7DF-6FAE-6916-BE58-98E623ED6F46");

    public static IEnumerable<Order> GetOrders()
    {
        var orders = new Order[]
        {
            new Order("John Smith", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)), AuthorizedOrganization1).SetId(Order1Id),
            new Order("Jane Doe", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)), AuthorizedOrganization1).SetId(Order2Id),
            new Order("Bob Johnson", DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-3)), AuthorizedOrganization2).SetId(Order3Id),
        };

        return orders;
    }

    public static IEnumerable<OrderItem> GetOrderItems(IEnumerable<Order> orders)
    {
        var ordersList = orders.ToList();
        var orderItems = new List<OrderItem>();

        // Add specific order items with known IDs for easier testing
        orderItems.AddRange(new[]
        {
            new OrderItem(Order1Id, "Laptop Computer", 1, 1299.99m).SetId(OrderItem1Id),
            new OrderItem(Order1Id, "Mouse", 2, 25.50m).SetId(OrderItem2Id),
            new OrderItem(Order2Id, "Office Chair", 1, 450.00m).SetId(OrderItem3Id),
        });

        // Generate deterministic order items for all orders
        orderItems.AddRange(Enumerable
            .Range(1, 100)
            .Select(id => new OrderItem(
                GetDeterministicOrder(ordersList, id).Id,
                $"Product #{id}",
                GetDeterministicQuantity(id),
                GetDeterministicUnitPrice(id))));

        return orderItems;
    }

    private static int GetDeterministicQuantity(int orderItemId)
    {
        // Use modulo to create deterministic quantity between 1 and 10
        return 1 + orderItemId % 10;
    }

    private static decimal GetDeterministicUnitPrice(int orderItemId)
    {
        // Use modulo to create deterministic price between 5.00 and 199.99
        return 5.00m + (orderItemId % 195);
    }

    private static Order GetDeterministicOrder(List<Order> orders, int orderItemId)
    {
        // Use modulo to deterministically assign orders based on order item ID
        return orders[orderItemId % orders.Count];
    }
}