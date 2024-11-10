using AutoMapper;
using repository.module.Models;
using repository.module.Models.Internal;

namespace repository.module.Profiles
{
    internal class EventDataMappingProfile : Profile
    {
        public EventDataMappingProfile()
        {
            CreateMap<EventData, EventDataInternal>()
                .ForMember(dest => dest.Event, opt => opt.Ignore())
                .ForMember(dest => dest.EventId, opt => opt.Ignore());

            CreateMap<EventDataInternal, EventData>();
        }
    }
}
