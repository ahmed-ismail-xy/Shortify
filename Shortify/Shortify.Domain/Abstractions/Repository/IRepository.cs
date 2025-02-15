using System.Linq.Expressions;

namespace Shortify.Domain.Abstractions.Repository;

public interface IRepository<TEntity> where TEntity : Entity
{
    IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetByIdAsync(Guid id);
    IEnumerable<TEntity> GetAll();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
