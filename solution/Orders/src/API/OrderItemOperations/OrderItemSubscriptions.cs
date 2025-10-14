using Application.OrderItemQueries;
using Domain.OrderItems;
using HotChocolate.Subscriptions;
using System.Runtime.CompilerServices;

namespace API.OrderItemOperations;

[SubscriptionType]
public class OrderItemSubscriptions
{
    public async IAsyncEnumerable<OrderItemNode> OnOrderItemCreatedStream(ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var sourceStream = await eventReceiver.SubscribeAsync<OrderItem>(nameof(OrderItemMutations.CreateOrderItem), cancellationToken);

        // This simulates a scenario where we could replay missed events from the data store. 
        yield return new OrderItem(Guid.NewGuid(), "Sample Product", 1, 10.00m).MapToDto();

        await Task.Delay(5000, cancellationToken);

        await foreach (OrderItem orderItem in sourceStream.ReadEventsAsync().WithCancellation(cancellationToken))
        {
            yield return orderItem.MapToDto();
        }
    }

    [Subscribe(With = nameof(OnOrderItemCreatedStream))]
    public OrderItemNode OnOrderItemCreated([EventMessage] OrderItemNode orderItem) => orderItem;
}