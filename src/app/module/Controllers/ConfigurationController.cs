using Microsoft.AspNetCore.Mvc;
using src.app.module.Models;
using src.app.module.Repositories;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<OutputConfigurationModel>>> GetConfigurations(CancellationToken cancellationToken)
        {
            var configurations = await _configurationRepository.GetConfigurationsAsync(cancellationToken);
            var outputModels = _mapper.Map<IEnumerable<OutputConfigurationModel>>(configurations);
            return Ok(outputModels);
        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<OutputConfigurationModel>> GetConfiguration(string uid, CancellationToken cancellationToken)
        {
            var dbModel = await _configurationRepository.GetConfigurationAsync(uid, cancellationToken);

            if (dbModel == null)
            {
                return NotFound();
            }

            var outputModel = _mapper.Map<OutputConfigurationModel>(dbModel);
            return Ok(outputModel);
        }

        [HttpPost("{uid}")]
        public async Task<IActionResult> SaveConfiguration(string uid, DbConfigurationModel model, CancellationToken cancellationToken)
        {
            if (model == null || model.Uid != uid)
            {
                return BadRequest("Invalid configuration data.");
            }

            await _configurationRepository.SaveConfigurationAsync(model, cancellationToken);
            return Ok();
        }
    }
}
