using AutoMapper;
using repository.module.Interfaces;
using repository.module.Models.Internal;
using repository.module.Models.Output;

namespace repository.module.Implementations
{
    internal class ConfigurationRepository : BaseRepository<Configuration, ConfigurationInternal>, IConfigurationRepository
    {
        public ConfigurationRepository(AppDbContext context, IMapper mapper) : base(context, mapper) { }

        private protected override ConfigurationInternal GetInternalModel(Configuration item)
        {
            return _mapper.Map<Configuration, ConfigurationInternal>(item);
        }

        private protected override Configuration GetOutputModel(ConfigurationInternal item)
        {
            return _mapper.Map<ConfigurationInternal, Configuration>(item);
        }
    }
}
