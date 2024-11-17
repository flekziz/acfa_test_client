using AutoMapper;
using repository.module.Models;
using app.module.Models;

namespace app.module.Profiles
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<EventDto, Event>();
        }
    }
}
