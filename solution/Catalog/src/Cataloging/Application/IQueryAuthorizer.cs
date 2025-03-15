using Cataloging.Domain;
using Common.Application.Authentication;
using Common.Domain;

namespace Cataloging.Application;

public interface IQueryAuthorizer
{
    Task<IQueryable<TEntity>> GetAuthorizedEntities<TEntity>(User user) where TEntity : Entity;
}