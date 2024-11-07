using System.Threading;
using System.Threading.Tasks;

namespace repository.module.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<TEntity[]> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity item, CancellationToken cancellationToken = default);
        void Remove(TEntity item);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
