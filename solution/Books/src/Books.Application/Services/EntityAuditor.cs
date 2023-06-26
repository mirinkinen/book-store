namespace Books.Application.Services;

public class EntityAuditor : IEntityAuditor, IDisposable
{
    private IList<(Type, Guid)> _entityIds = new List<(Type, Guid)>();
    private bool _disposedValue;

    public EntityAuditor()
    {

    }

    public void AddId(Type type, Guid id)
    {
        _entityIds.Add(new(type, id));
    }

    public Task WriteAuditMessage()
    {
        // Do whatever with audit messages.
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}