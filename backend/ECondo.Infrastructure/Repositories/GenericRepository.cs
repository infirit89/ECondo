using System.Linq.Expressions;
using ECondo.Application.Repositories;
using ECondo.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Infrastructure.Repositories
{
    internal class GenericRepository<TEntity>(ECondoDbContext dbContext) : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                         (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }

        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task DeleteByIdAsync(object id)
        {
            TEntity entity = (await _dbSet.FindAsync(id))!;
            _dbSet.Remove(entity);
        }

        public void Delete(TEntity entityToDelete)
        {
            _dbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            _dbSet.Update(entityToUpdate);
        }

        public IQueryable<TEntity> GetQueryable() => _dbSet.AsQueryable();

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (string includeProperty in includeProperties.Split
                         (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return filter is null ?
                query.FirstOrDefaultAsync() :
                query.FirstOrDefaultAsync(filter);
        }
    }
}
