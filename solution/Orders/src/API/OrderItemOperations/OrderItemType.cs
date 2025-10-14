using Application.OrderItemQueries;

namespace API.OrderItemOperations;

[ObjectType<OrderItemNode>]
public static partial class OrderItemType
{
    // OrderItem is a simple entity without complex relationships
    // All fields are already defined in OrderItemNode
    // No additional field resolvers needed for basic functionality
}