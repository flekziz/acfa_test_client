using AutoMapper;
using repository.module.Interfaces;
using repository.module.Models.Internal;
using repository.module.Models;
using Microsoft.EntityFrameworkCore;

namespace repository.module.Implementations
{
    internal class ConfigurationRepository : BaseRepository<Configuration, ConfigurationInternal>, IConfigurationRepository
    {
        public ConfigurationRepository(IDbContextFactory<AppDbContext> dbContextFactory, IMapper mapper) 
            : base(dbContextFactory, mapper) { }

        private protected override ConfigurationInternal GetInternalModel(Configuration item)
        {
            return _mapper.Map<Configuration, ConfigurationInternal>(item);
        }

        private protected override Configuration GetOutputModel(ConfigurationInternal item)
        {
            return _mapper.Map<ConfigurationInternal, Configuration>(item);
        }

        private protected override string GetSortKey(ConfigurationInternal item)
        {
            return item.Uid;
        }
    }
}
