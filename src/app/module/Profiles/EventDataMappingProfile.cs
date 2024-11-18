using AutoMapper;
using repository.module.Models;
using app.module.Models;

namespace app.module.Profiles
{
    public class EventDataMappingProfile : Profile
    {
        public EventDataMappingProfile()
        {
            CreateMap<EventData, EventDataDto>();
            CreateMap<EventDataDto, EventData>();
        }
    }
}
