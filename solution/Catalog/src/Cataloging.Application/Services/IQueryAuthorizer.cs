using Cataloging.Domain.SeedWork;

namespace Cataloging.Application.Services;

public interface IQueryAuthorizer
{
    IQueryable<TEntity> GetAuthorizedEntities<TEntity>() where TEntity : Entity;
}