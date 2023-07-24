using System.Globalization;
using Cataloging.Domain.Authors;
using Common.Application.Messages;
using Wolverine;
using Wolverine.Attributes;

namespace Cataloging.Application.Requests.Ping;

[Transactional]
public static class PingHandler
{
    [Transactional]
    public static async Task<Pong> Handle(Common.Application.Messages.Ping ping, Envelope envelope, IAuthorRepository 
    repository, CancellationToken token)
    {
        if (envelope.Attempts < 3)
        {
            throw new InvalidOperationException("Just can't right now...");
        }

        var author = await repository.GetAuthorById(Guid.Parse("de80adbd-962c-43eb-a092-0b2bba216f0d"), token);
        var firstname = author.FirstName.Substring(0, 5) + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
        author.Update(firstname, author.LastName, author.Birthday);

        return new Pong(ping.Number + 1);
    }
}