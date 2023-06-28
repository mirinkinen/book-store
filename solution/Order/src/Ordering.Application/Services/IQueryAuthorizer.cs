using Ordering.Domain.SeedWork;

namespace Ordering.Application.Services;

public interface IQueryAuthorizer
{
    IQueryable<TEntity> GetAuthorizedEntities<TEntity>() where TEntity : Entity;
}