using Microsoft.AspNetCore.Mvc;
using repository.module.Interfaces;
using repository.module.Models;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace src.app.module.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : BaseController<Event>
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository, IMapper mapper)
            : base(eventRepository, mapper)
        {
            _eventRepository = eventRepository;
        }

        protected override bool IsValidUid(Event model, string uid)
        {
            return model.Uid == uid;
        }
    }
}
