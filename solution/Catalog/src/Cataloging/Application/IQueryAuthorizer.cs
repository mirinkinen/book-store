using Cataloging.Domain;
using Common.Domain;

namespace Cataloging.Application;

public interface IQueryAuthorizer
{
    Task<IQueryable<TEntity>> GetAuthorizedEntities<TEntity>() where TEntity : Entity;
}