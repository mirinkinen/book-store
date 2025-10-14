using Infra.Database;

namespace TestData;

public static class DataSeeder
{
    public static async Task SeedDataAsync(OrdersDbContext ordersDbContext)
    {
        ArgumentNullException.ThrowIfNull(ordersDbContext, nameof(ordersDbContext));

        var orders = TestDataContainer.GetOrders().ToList();

        // If not already seeded.
        if (!ordersDbContext.Orders.Any())
        {
            await ordersDbContext.AddRangeAsync(orders);
            await ordersDbContext.SaveChangesAsync();
        }

        // If not already seeded.
        if (!ordersDbContext.OrderItems.Any())
        {
            var orderItems = TestDataContainer.GetOrderItems(ordersDbContext.Orders.ToList());
            await ordersDbContext.AddRangeAsync(orderItems);
            await ordersDbContext.SaveChangesAsync();
        }
    }
}