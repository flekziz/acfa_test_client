using Microsoft.AspNetCore.Mvc;
using repository.module.Interfaces;
using repository.module.Models;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace src.app.module.Controllers
{
    [ApiController]
    [Route("api/configurations")]
    public class ConfigurationController : BaseController<Configuration>
    {
        private readonly IConfigurationRepository _configurationRepository;

        public ConfigurationController(IConfigurationRepository configurationRepository, IMapper mapper)
            : base(configurationRepository, mapper)
        {
            _configurationRepository = configurationRepository;
        }

        protected override bool IsValidUid(Configuration model, string uid)
        {
            return model.Uid == uid;
        }
    }
}
