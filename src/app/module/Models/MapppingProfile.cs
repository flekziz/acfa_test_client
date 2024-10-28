using System.Linq;
using AutoMapper;
using src.app.module.Models;

namespace src.app.module.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DbConfigurationModel, OutputConfigurationModel>()
                .ForMember(dest => dest.Properties, opt => opt.MapFrom(src =>
                src.Properties.ToDictionary(p => p.Id, p => p.ValueString)));
        }
    }
}
