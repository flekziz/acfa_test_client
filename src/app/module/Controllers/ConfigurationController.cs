using Microsoft.AspNetCore.Mvc;
using repository.module.Interfaces;
using repository.module.Models;
using app.module.Models;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace app.module.Controllers
{
    [ApiController]
    [Route("api/configurations")]
    public class ConfigurationController : BaseController<Configuration, ConfigurationDto>
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationController(IConfigurationRepository configurationRepository, IMapper mapper, ILogger logger)
            : base(configurationRepository, mapper, logger)
        {
            _configurationRepository = configurationRepository;
        }
    }
}
