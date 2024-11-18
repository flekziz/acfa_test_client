using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repository.module.Interfaces;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace app.module.Controllers
{
    public abstract class BaseController<TModel, TDto> : ControllerBase
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
        public async Task<ActionResult<IEnumerable<TDto>>> GetAll(
            [FromQuery] int? pageNumber, 
            [FromQuery] int? pageSize, 
            CancellationToken cancellationToken)
        {
            try
            {
                if(pageNumber.HasValue && pageSize.HasValue)
                {
                    if (pageNumber <= 0 || pageSize <= 0)
                    {
                        _logger.LogWarning("Invalid pagination parameters: pageNumber={PageNumber}, pageSize={PageSize}", pageNumber, pageSize);
                        return BadRequest("Pagination parameters must be greater than zero.");
                    }

                    _logger.LogInformation("Retrieving paginated items of type {TModel} - PageNumber: {PageNumber}, PageSize: {PageSize}", typeof(TModel).Name, pageNumber, pageSize);

                    var pagedItems = await _repository.GetAllAsync(pageNumber.Value, pageSize.Value, cancellationToken);
                    var pagedDtos = _mapper.Map<IEnumerable<TDto>>(pagedItems);

                    _logger.LogInformation("Retrieved {Count} paginated items of type {TModel}", pagedItems?.Length ?? 0, typeof(TModel).Name);

                    return Ok(pagedDtos);
                }
                else
                {
                    _logger.LogInformation("Retrieving all items of type {TModel}", typeof(TModel).Name);

                    var items = await _repository.GetAllAsync(cancellationToken);
                    var itemDtos = _mapper.Map<IEnumerable<TDto>>(items);

                    _logger.LogInformation("Retrieved {Count} items of type {TModel}", items?.Length ?? 0, typeof(TModel).Name);

                    return Ok(itemDtos);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving items of type {TModel}", typeof(TModel).Name);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<TDto>> GetById(string uid, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting to retrieve item of type {TModel} with UID: {UID}", typeof(TModel).Name, uid);

                var item = await _repository.GetByIdAsync(uid, cancellationToken);
                if (item == null)
                {
                    _logger.LogWarning("Item of type {TModel} with UID: {UID} was not found.", typeof(TModel).Name, uid);
                    return NotFound();
                }

                var itemDto = _mapper.Map<TDto>(item);

                _logger.LogInformation("Retrieved item of type {TModel} with UID: {UID}", typeof(TModel).Name, uid);

                return Ok(itemDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving item of type {TModel} with UID: {UID}", typeof(TModel).Name, uid);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("{uid}")]
        public async Task<IActionResult> Save(TDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model validation failed for {TModel}", typeof(TModel).Name);
                    return BadRequest(ModelState);
                }

                var model = _mapper.Map<TModel>(dto);

                _logger.LogInformation("Saving item of type {TModel}", typeof(TModel).Name);

                await _repository.AddAsync(model, cancellationToken);

                _logger.LogInformation("Successfully saved item of type {TModel}", typeof(TModel).Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving item of type {TModel}", typeof(TModel).Name);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkSave(TDto[] dtos, CancellationToken cancellationToken)
        {
            try
            {
                if (dtos?.Any() != true)
                {
                    _logger.LogWarning("Bulk save failed because the list of items is empty or null.");
                    return BadRequest("The list of items is empty or null.");
                }

                _logger.LogInformation("Starting bulk save for {Count} items of type {TModel}", dtos.Length, typeof(TModel).Name);

                foreach (var model in dtos)
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

                var models = _mapper.Map<IEnumerable<TModel>>(dtos);

                await _repository.AddRangeAsync(cancellationToken, models.ToArray());
                _logger.LogInformation("Successfully performed bulk save for {Count} items of type {TModel}", dtos.Length, typeof(TModel).Name);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during bulk save for items of type {TModel}", typeof(TModel).Name);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete(string uid, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting item of type {TModel} with UID: {UID}", typeof(TModel).Name, uid);
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
