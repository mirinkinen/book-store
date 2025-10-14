using Application.OrderQueries;
using Domain.Orders;
using HotChocolate.Subscriptions;
using System.Runtime.CompilerServices;

namespace API.OrderOperations;

[SubscriptionType]
public class OrderSubscriptions
{
    public async IAsyncEnumerable<OrderNode> OnOrderCreatedStream(ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var sourceStream = await eventReceiver.SubscribeAsync<Order>(nameof(OrderMutations.CreateOrder), cancellationToken);

        // This simulates a scenario where we could replay missed events from the data store. 
        yield return new Order("Sample Customer", DateOnly.FromDateTime(DateTime.UtcNow), Guid.NewGuid()).MapToDto();

        await Task.Delay(5000, cancellationToken);

        await foreach (Order order in sourceStream.ReadEventsAsync().WithCancellation(cancellationToken))
        {
            yield return order.MapToDto();
        }
    }

    [Subscribe(With = nameof(OnOrderCreatedStream))]
    public OrderNode OnOrderCreated([EventMessage] OrderNode order) => order;
}