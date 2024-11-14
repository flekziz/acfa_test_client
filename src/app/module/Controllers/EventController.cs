using Microsoft.AspNetCore.Mvc;
using repository.module.Interfaces;
using repository.module.Models;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace src.app.module.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : BaseController<Event>
    {
        private readonly IEventRepository _eventRepository;

        public EventController(IEventRepository eventRepository, IMapper mapper, ILogger logger)
            : base(eventRepository, mapper, logger)
        {
            _eventRepository = eventRepository;
        }
    }
}
