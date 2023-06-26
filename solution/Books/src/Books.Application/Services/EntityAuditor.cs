namespace Books.Application.Services;

public record TypeId(Type type, Guid Id);

public class EntityAuditor : IEntityAuditor, IDisposable
{
    private List<TypeId> _entityIds = new();
    public IReadOnlyList<TypeId> EntityIds => _entityIds.AsReadOnly();
    private bool _disposedValue;

    public void AddId(Type type, Guid id)
    {
        _entityIds.Add(new TypeId(type, id));
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