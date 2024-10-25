using Microsoft.AspNetCore.Mvc;
using src.app.module.Models;
using src.app.module.Repositories;
using AutoMapper;

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

        [HttpGet("{uid}")]
        public ActionResult<OutputConfigurationModel> GetConfiguration(string uid)
        {
            var dbModel = _configurationRepository.GetConfiguration(uid);

            if (dbModel == null)
            {
                return NotFound();
            }

            var outputModel = _mapper.Map<OutputConfigurationModel>(dbModel);
            return Ok(outputModel);
        }

        [HttpPost("{uid}")]
        public IActionResult SaveConfiguration(string uid, [FromBody] DbConfigurationModel model)
        {
            if (model == null || model.Uid != uid)
            {
                return BadRequest("Invalid configuration data.");
            }

            _configurationRepository.SaveConfiguration(model);
            return Ok();
        }
    }
}
