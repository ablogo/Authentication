using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();

        Task<int> SaveChanges();

        bool Commit();

        void Rollback();

        IRepository<T> Repository<T>();

    }
}
