namespace Cataloging.IntegrationTests;

public class WebApplicationFactoryWarmer : IAsyncDisposable
{
    private readonly List<Task<IntegrationWebApplicationFactory>> _factoryLines = new();
    private int _linePointer;
    private readonly object _threadLock = new();

    public WebApplicationFactoryWarmer()
    {
        for (var i = 0; i < 3; i++)
        {
            var factory = CreateFactory();

            _factoryLines.Add(factory);
        }
    }

    private static Task<IntegrationWebApplicationFactory> CreateFactory()
    {
        return Task.Run(() =>
        {
            var factory = new IntegrationWebApplicationFactory();
            factory.CreateClient();

            return factory;
        });
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var factoryLine in _factoryLines)
        {
            var factory = await factoryLine;
            await factory.DisposeAsync();
        }
    }


    internal Task<IntegrationWebApplicationFactory> GetFactory()
    {
        lock (_threadLock)
        {
            // Get ready factory...
            var factory = _factoryLines[_linePointer];

            // ... and start making a new factory at the same factory line.
            _factoryLines[_linePointer] = CreateFactory();

            _linePointer++;
            if (_linePointer >= _factoryLines.Count)
            {
                _linePointer = 0;
            }

            return factory;
        }
    }
}