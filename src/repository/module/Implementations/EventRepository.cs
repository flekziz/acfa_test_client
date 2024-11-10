using repository.module.Models;
using repository.module.Interfaces;
using repository.module.Models.Internal;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace repository.module.Implementations
{
    internal class EventRepository : BaseRepository<Event, EventInternal>, IEventRepository
    {
        public EventRepository(IDbContextFactory<AppDbContext> dbContextFactory, IMapper mapper) 
            : base(dbContextFactory, mapper) { }

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
