using System.Diagnostics;

namespace Books.Application.Services;

public record TypeId(string Type, Guid Id);

public class EntityAuditor : IEntityAuditor, IDisposable
{
    private List<TypeId> _entityIds = new();
    public IReadOnlyList<TypeId> EntityIds => _entityIds.AsReadOnly();
    private bool _disposedValue;

    public void AddId(Type type, Guid id)
    {
        ArgumentNullException.ThrowIfNull(type);

        _entityIds.Add(new TypeId(type.ToString(), id));
    }

    public void AddId(string type, Guid id)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (_entityIds.Any(t => t.Type == type && t.Id == id))
        {
            return;
        }

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
                Debug.WriteLine("");
                Debug.WriteLine(new string('-', 30));
                Debug.WriteLine("AUDIT LOGGING");

                foreach (var entity in _entityIds)
                {
                    Debug.WriteLine($"{entity.Type}: {entity.Id}");
                }

                Debug.WriteLine(new string('-', 30));
                Debug.WriteLine("");
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