namespace Cataloging.IntegrationTests;

public class WebApplicationFactoryWarmer : IAsyncDisposable
{
    private readonly List<Task<IntegrationWebApplicationFactory>> _factoryPipelines = new();
    private int _pipelinePointer;
    private readonly object _threadLock = new();

    public WebApplicationFactoryWarmer()
    {
        for (var i = 0; i < 3; i++)
        {
            var factory = CreateFactory();

            _factoryPipelines.Add(factory);
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
        foreach (var pipeline in _factoryPipelines)
        {
            var factory = await pipeline;
            await factory.DisposeAsync();
        }
    }


    internal Task<IntegrationWebApplicationFactory> GetFactory()
    {
        lock (_threadLock)
        {
            // Get ready factory...
            var factory = _factoryPipelines[_pipelinePointer];

            // ... and start making a new factory at the same pipeline.
            _factoryPipelines[_pipelinePointer] = CreateFactory();

            _pipelinePointer++;
            if (_pipelinePointer >= _factoryPipelines.Count)
            {
                _pipelinePointer = 0;
            }

            return factory;
        }
    }
}