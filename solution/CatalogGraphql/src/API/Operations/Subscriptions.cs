using Application.Types;
using Domain;
using HotChocolate.Subscriptions;
using System.Runtime.CompilerServices;

namespace API.Operations;

[SubscriptionType]
public class Subscriptions
{
    public async IAsyncEnumerable<Author> OnAuthorCreatedStream(ITopicEventReceiver eventReceiver, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var sourceStream = await eventReceiver.SubscribeAsync<Author>(nameof(AuthorMutations.CreateAuthor), cancellationToken);

        // This simulates a scenario where we could replay missed events from the data store. 
        yield return new Author("Some first name", "Some last name", DateOnly.FromDateTime(DateTime.UtcNow), Guid.NewGuid());

        await Task.Delay(5000, cancellationToken);

        await foreach (Author author in sourceStream.ReadEventsAsync().WithCancellation(cancellationToken))
        {
            yield return author;
        }
    }

    [Subscribe(With = nameof(OnAuthorCreatedStream))]
    public Author OnAuthorCreated([EventMessage] Author author) => author;
}