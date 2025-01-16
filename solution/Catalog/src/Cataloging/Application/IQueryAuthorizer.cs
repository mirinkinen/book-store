using Cataloging.Domain;

namespace Cataloging.Application;

public interface IQueryAuthorizer
{
    IQueryable<TEntity> GetAuthorizedEntities<TEntity>() where TEntity : Entity;
}