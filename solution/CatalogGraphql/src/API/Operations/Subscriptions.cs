using Application.Types;
using HotChocolate.Subscriptions;
using System.Runtime.CompilerServices;

namespace API.Operations;

public class Subscriptions
{
    public async IAsyncEnumerable<AuthorDto> OnAuthorCreatedStream(ITopicEventReceiver eventReceiver, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var sourceStream = await eventReceiver.SubscribeAsync<AuthorDto>(nameof(AuthorMutations.CreateAuthor), cancellationToken);

        // This simulates a scenario where we could replay missed events from the data store. 
        yield return new AuthorDto(Guid.NewGuid(), "Some first name", "Some last name", DateTime.Now, Guid.NewGuid());

        await Task.Delay(5000, cancellationToken);

        await foreach (AuthorDto author in sourceStream.ReadEventsAsync().WithCancellation(cancellationToken))
        {
            yield return author;
        }
    }

    [Subscribe(With = nameof(OnAuthorCreatedStream))]
    public AuthorDto OnAuthorCreated([EventMessage] AuthorDto author) => author;
}