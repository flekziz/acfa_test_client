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
        protected readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        protected readonly IMapper _mapper;

        protected BaseRepository(IDbContextFactory<AppDbContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        private protected abstract TEntityIn GetInternalModel(TEntityOut item);
        private protected abstract TEntityOut GetOutputModel(TEntityIn item);
        private protected abstract string GetKey(TEntityIn item);

        public async Task AddAsync(TEntityOut item, CancellationToken cancellationToken = default)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var internalItem = GetInternalModel(item);
            await context.AddAsync(internalItem, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddRangeAsync(CancellationToken cancellationToken = default, params TEntityOut[] items)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var internalItems = items.Select(item => GetInternalModel(item)).ToArray();
            await context.AddRangeAsync(internalItems, cancellationToken: cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntityOut[]> GetAllAsync(CancellationToken cancellationToken = default)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var internalItems = await context.Set<TEntityIn>().ToArrayAsync(cancellationToken);
            var outputItems = internalItems.Select(item => GetOutputModel(item)).ToArray();
            return outputItems;
        }

        public async Task<TEntityOut[]> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var internalItemsPage = await context.Set<TEntityIn>()
                .OrderBy(i => GetKey(i))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            var outputItemsPage = internalItemsPage.Select(item => GetOutputModel(item)).ToArray();
            return outputItemsPage;
        }

        public async Task<TEntityOut> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var internalItem = await context.FindAsync<TEntityIn>(id, cancellationToken);
            return GetOutputModel(internalItem);
        }

        public async Task RemoveAsync(TEntityOut item, CancellationToken cancellationToken = default)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var internalItem = GetInternalModel(item);
            internalItem = await context.FindAsync<TEntityIn>(GetKey(internalItem));
            if (internalItem is not null)
            {
                context.Remove(internalItem);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
