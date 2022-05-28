using Authentication.Core.Interfaces;
using Authentication.DataAccess.DbContext;
using System.Data.Common;

namespace Authentication.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _dbContext;
        private DbTransaction _transaction;

        protected Dictionary<string, dynamic> Repositories;

        public UnitOfWork(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
            Repositories = new Dictionary<string, dynamic>();
        }

        public async Task BeginTransaction()
        {
            _transaction = (DbTransaction)await _dbContext.Database.BeginTransactionAsync();
        }

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public IRepository<T> Repository<T>()
        {
            if (Repositories == null)
            {
                Repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(T).Name;

            if (Repositories.ContainsKey(type))
            {
                return (IRepository<T>)Repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            Repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext));

            return Repositories[type];
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
