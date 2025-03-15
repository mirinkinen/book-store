namespace Common.Domain;

public interface IQueryAuthorizerRepository<out TEntity> where TEntity : Entity
{
    IQueryable<TEntity> GetQuery();
}