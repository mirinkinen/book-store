namespace Books.Application.Services;

public interface IEntityAuditor
{
    public void AddId(Type type, Guid id);

    public Task WriteAuditMessage();
}