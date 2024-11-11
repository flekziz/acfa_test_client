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
    [Route("api/configurations")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMapper _mapper;

        public ConfigurationController(IConfigurationRepository configurationRepository, IMapper mapper)
        {
            _configurationRepository = configurationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Configuration>>> GetConfigurations(CancellationToken cancellationToken)
        {
            var configurations = await _configurationRepository.GetAllAsync(cancellationToken);

            var outputModels = _mapper.Map<IEnumerable<Configuration>>(configurations);
            return Ok(outputModels);
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<Configuration>> GetConfiguration(string uid, CancellationToken cancellationToken)
        {
            var dbModel = await _configurationRepository.GetByIdAsync(uid, cancellationToken);

            var outputModel = _mapper.Map<Configuration>(dbModel);
            return Ok(outputModel);
        }

        //нет доступа к internal классу для маппинга

        //[HttpPost("{uid}")]
        //public async Task<IActionResult> SaveConfiguration(string uid, Configuration model, CancellationToken cancellationToken)
        //{
        //    if (model.Uid != uid)
        //    {
        //        return BadRequest("Invalid configuration data.");
        //    }

        //    var internalModel = _mapper.Map<ConfigurationInternal>(model);
        //    await _configurationRepository.AddAsync(internalModel, cancellationToken);
        //    return Ok();
        //}

        //[HttpPost("bulk")]
        //public async Task<IActionResult> SaveConfigurations(string uid, Configuration[] models, CancellationToken cancellationToken)
        //{
        //    if (models == null || models.Length == 0)
        //    {
        //        return BadRequest("Invalid configuration data.");
        //    }

        //    var internalModels = _mapper.Map<ConfigurationInternal[]>(models);
        //    await _configurationRepository.AddRangeAsync(cancellationToken, internalModels);
        //    return Ok();
        //}

        [HttpDelete("{uid}")]
        public async Task<IActionResult> DeleteConfiguration(string uid, CancellationToken cancellationToken)
        {
            var configuration = await _configurationRepository.GetByIdAsync(uid, cancellationToken);

            if(configuration == null)
            {
                return NotFound();
            }

            await _configurationRepository.RemoveAsync(configuration);
            return Ok();
        }
    }
}
