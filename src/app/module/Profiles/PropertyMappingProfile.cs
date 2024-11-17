using AutoMapper;
using repository.module.Models;
using app.module.Models;

namespace repository.module.Profiles
{
    internal class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties));

            CreateMap<PropertyDto, Property>();
        }
    }
}
