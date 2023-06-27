namespace Books.Application.Services;

public interface IEntityAuditor
{
    public void AddId(Type type, Guid id);

    public void AddId(string type, Guid id);

    public Task WriteAuditMessage();

    public IReadOnlyList<TypeId> EntityIds { get; }
}