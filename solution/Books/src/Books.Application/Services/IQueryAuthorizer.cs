using Books.Domain.SeedWork;

namespace Books.Application;

public interface IQueryAuthorizer
{
    IQueryable<TEntity> GetAuthorizedEntities<TEntity>() where TEntity : Entity;
}