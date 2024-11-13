using AutoMapper;
using repository.module.Models;
using repository.module.Models.Internal;

namespace repository.module.Profiles
{
    internal class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            CreateMap<Property, PropertyInternal>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Id));

            CreateMap<PropertyInternal, Property>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Name));
        }
    }
}
