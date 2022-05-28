using Authentication.Core.Interfaces;
using Authentication.DataAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AuthDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AuthDbContext dbContext)
        {
            _context = dbContext;
            _dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task Add(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task Delete(object id)
        {
            T entity = _dbSet.Find(id);
            await Delete(entity);
        }

        public async Task Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public async Task Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> Query()
        {
            return _dbSet;
        }
    }
}
