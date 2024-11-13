using Microsoft.AspNetCore.Mvc;
using repository.module.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace src.app.module.Controllers
{
    public abstract class BaseController<TModel> : ControllerBase
    {
        private readonly IRepository<TModel> _repository;
        private readonly IMapper _mapper;

        protected BaseController(IRepository<TModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll(CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync(cancellationToken);
            return Ok(items);
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<TModel>> GetById(string uid, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(uid, cancellationToken);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost("{uid}")]
        public async Task<IActionResult> Save(string uid, TModel model, CancellationToken cancellationToken)
        {
            if (!IsValidUid(model, uid)) return BadRequest("Invalid data.");

            await _repository.AddAsync(model, cancellationToken);
            return Ok();
        }

        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete(string uid, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(uid, cancellationToken);
            if (item == null) return NotFound();

            await _repository.RemoveAsync(item);
            return Ok();
        }

        protected abstract bool IsValidUid(TModel model, string uid);
    }
}
