using repository.module.Models.Output;
using repository.module.Interfaces;
using repository.module.Models.Internal;
using AutoMapper;

namespace repository.module.Implementations
{
    internal class EventRepository : BaseRepository<Event, EventInternal>, IEventRepository
    {
        public EventRepository(AppDbContext context, IMapper mapper) : base(context, mapper) { }

        private protected override EventInternal GetInternalModel(Event item)
        {
            return _mapper.Map<Event, EventInternal>(item);
        }

        private protected override Event GetOutputModel(EventInternal item)
        {
            return _mapper.Map<EventInternal, Event>(item);
        }
    }
}
