using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using repository.module.Interfaces;

namespace repository.module.Implementations
{
    internal abstract class BaseRepository<TEntityOut, TEntityIn> : IRepository<TEntityOut> where TEntityIn : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntityIn> _dbSet;
        protected readonly IMapper _mapper;

        protected BaseRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = context.Set<TEntityIn>();
            _mapper = mapper;
        }

        private protected abstract TEntityIn GetInternalModel(TEntityOut item);
        private protected abstract TEntityOut GetOutputModel(TEntityIn item);

        public async Task AddAsync(TEntityOut item, CancellationToken cancellationToken = default)
        {
            var internalItem = GetInternalModel(item);
            await _context.AddAsync(internalItem, cancellationToken);
        }

        public async Task<TEntityOut[]> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var internalItems = await _dbSet.ToArrayAsync(cancellationToken);
            var outputItems = internalItems.Select(item => GetOutputModel(item)).ToArray();
            return outputItems;
        }

        public async Task<TEntityOut> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var internalItem = await _dbSet.FindAsync(id, cancellationToken);
            return GetOutputModel(internalItem);
        }

        public void Remove(TEntityOut item)
        {
            var internalItem = GetInternalModel(item);
            _dbSet.Remove(internalItem);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
