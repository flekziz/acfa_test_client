using AutoMapper;
using repository.module.Models;
using repository.module.Models.Internal;

namespace repository.module.Profiles
{
    internal class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            CreateMap<Event, EventInternal>();

            CreateMap<EventInternal, Event>();
        }
    }
}
