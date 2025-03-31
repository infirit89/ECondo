using System.Linq.Expressions;

namespace ECondo.Application.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");

    Task<TEntity?> GetByIdAsync(object id);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        string includeProperties = "");

    Task InsertAsync(TEntity entity);
    Task DeleteByIdAsync(object id);
    void Delete(TEntity entityToDelete);
    void Update(TEntity entityToUpdate);
}
