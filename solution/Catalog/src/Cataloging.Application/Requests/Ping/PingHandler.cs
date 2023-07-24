using Common.Application.Messages;
using Wolverine;
using Wolverine.Attributes;

namespace Cataloging.Application.Requests.Ping;

public static class PingHandler
{
    [Transactional]
    public static Pong Handle(Common.Application.Messages.Ping ping)
    {
        return new Pong(ping.Number + 1);
    }
}