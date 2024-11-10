using AutoMapper;
using repository.module.Models;
using repository.module.Models.Internal;

namespace repository.module.Profiles
{
    internal class ConfigurationMappingProfile : Profile
    {
        public ConfigurationMappingProfile()
        {
            CreateMap<Configuration, ConfigurationInternal>()
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.ParentUid, opt => opt.Ignore());

            CreateMap<ConfigurationInternal, Configuration>();
        }
    }
}
