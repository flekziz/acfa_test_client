using Microsoft.AspNetCore.Mvc;
using repository.module.Interfaces;
using repository.module.Models.Internal;
using repository.module.Implementations;
using repository.module.Profiles;
using repository.module.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace src.app.module.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventController(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents(CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetAllAsync(cancellationToken);

            var outputModels = _mapper.Map<IEnumerable<Event>>(events);
            return Ok(outputModels);
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<Event>> GetEvent(string uid, CancellationToken cancellationToken)
        {
            var dbModel = await _eventRepository.GetByIdAsync(uid, cancellationToken);

            var outputModel = _mapper.Map<Event>(dbModel);
            return Ok(outputModel);
        }

        //нет доступа к internal классу для маппинга

        //[HttpPost("{uid}")]
        //public async Task<IActionResult> SaveEvent(string uid, Event model, CancellationToken cancellationToken)
        //{
        //    if (model.Uid != uid)
        //    {
        //        return BadRequest("Invalid event data.");
        //    }

        //    var internalModel = _mapper.Map<EventInternal>(model);
        //    await _eventRepository.AddAsync(internalModel, cancellationToken);
        //    return Ok();
        //}

        //[HttpPost("bulk")]
        //public async Task<IActionResult> SaveEvents(string uid, Event[] models, CancellationToken cancellationToken)
        //{
        //    if (models == null || models.Length == 0)
        //    {
        //        return BadRequest("Invalid event data.");
        //    }

        //    var internalModels = _mapper.Map<EventInternal[]>(models);
        //    await _eventRepository.AddRangeAsync(cancellationToken, internalModels);
        //    return Ok();
        //}

        [HttpDelete("{uid}")]
        public async Task<IActionResult> DeleteEvent(string uid, CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetByIdAsync(uid, cancellationToken);

            if (events == null)
            {
                return NotFound();
            }

            await _eventRepository.RemoveAsync(events);
            return Ok();
        }
    }
}
