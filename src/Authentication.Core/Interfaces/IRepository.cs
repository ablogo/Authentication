using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IRepository<T>
    {
        Task<T> GetById(object id);

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        Task Add(T entity);

        Task Add(IEnumerable<T> entities);

        Task Update(T entity);

        Task Delete(object id);

        Task Delete(T entity);

        IQueryable<T> Query();
    }
}
