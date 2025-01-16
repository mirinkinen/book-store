using Ordering.Domain;

namespace Ordering.Application;

public interface IQueryAuthorizer
{
    IQueryable<TEntity> GetAuthorizedEntities<TEntity>() where TEntity : Entity;
}