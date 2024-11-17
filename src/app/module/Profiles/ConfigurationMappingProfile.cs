using AutoMapper;
using repository.module.Models;
using app.module.Models;

namespace repository.module.Profiles
{
    internal class ConfigurationMappingProfile : Profile
    {
        public ConfigurationMappingProfile()
        {
            CreateMap<Configuration, ConfigurationDto>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties))
                .ForMember(dest => dest.Configurations, opt => opt.MapFrom(src => src.Configurations));

            CreateMap<ConfigurationDto, Configuration>();
        }
    }
}
