using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repository.module.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace src.app.module.Controllers
{
    public abstract class BaseController<TModel> : ControllerBase
    {
        private readonly IRepository<TModel> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;   

        protected BaseController(IRepository<TModel> repository, IMapper mapper, ILogger logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to retrieve all items of type {TModel}", typeof(TModel).Name);
            
            var items = await _repository.GetAllAsync(cancellationToken);
           
            _logger.LogInformation("Retrieved {Count} items of type {TModel}", items?.Count() ?? 0, typeof(TModel).Name);

            return Ok(items);
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<TModel>> GetById(string uid, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting to retrieve item of type {TModel} with UID: {UID}", typeof(TModel).Name, uid);

            var item = await _repository.GetByIdAsync(uid, cancellationToken);
            if (item == null) {
                _logger.LogWarning("Item of type {TModel} with UID: {UID} was not found.", typeof(TModel).Name, uid);
                return NotFound();
            }

            _logger.LogInformation("Retrieved item of type {TModel} with UID: {UID}", typeof(TModel).Name, uid);

            return Ok(item);
        }

        [HttpPost("{uid}")]
        public async Task<IActionResult> Save(TModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed for {TModel}", typeof(TModel).Name);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Saving item of type {TModel}", typeof(TModel).Name);

            await _repository.AddAsync(model, cancellationToken);

            _logger.LogInformation("Successfully saved item of type {TModel}", typeof(TModel).Name);
            return Ok();
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkSave(TModel[] models, CancellationToken cancellationToken)
        {
            if (models?.Any() != true)
            {
                _logger.LogWarning("Bulk save failed because the list of items is empty or null.");
                return BadRequest("The list of items is empty or null.");
            }

            _logger.LogInformation("Starting bulk save for {Count} items of type {TModel}", models.Length, typeof(TModel).Name);

            foreach (var model in models)
            {
                if (model == null)
                {
                    _logger.LogWarning("One of the items in the collection is null.");
                    ModelState.AddModelError(string.Empty, "One of the items in the collection is null.");
                    continue;
                }
                TryValidateModel(model);
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bulk save validation failed for items of type {TModel}", typeof(TModel).Name);
                return BadRequest(ModelState);
            }

            await _repository.AddRangeAsync(cancellationToken, models);
            _logger.LogInformation("Successfully performed bulk save for {Count} items of type {TModel}", models.Length, typeof(TModel).Name);

            return Ok();
        }


        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete(string uid, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting item of type {TModel} with UID: {UID}", typeof(TModel).Name, uid);

            var item = await _repository.GetByIdAsync(uid, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning("Item of type {TModel} with UID: {UID} was not found for deletion.", typeof(TModel).Name, uid);
                return NotFound();
            }
            
            await _repository.RemoveAsync(item);
            _logger.LogInformation("Successfully deleted item of type {TModel} with UID: {UID}", typeof(TModel).Name, uid);

            return Ok();
        }
    }
}
