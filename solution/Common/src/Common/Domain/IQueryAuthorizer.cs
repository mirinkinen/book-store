using Common.Application.Authentication;

namespace Common.Domain;

public interface IQueryAuthorizer<out TEntity> where TEntity : Entity
{
    IQueryable<TEntity> GetQuery(User user);
}