using src.app.module.Models;
using System.Collections.Generic;
using System.Linq;
using src.app.module.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace src.app.module.Repositories
{
    public class InMemoryConfigurationRepository : IConfigurationRepository
    {
        private readonly List<DbConfigurationModel> _configurations = new();

        public InMemoryConfigurationRepository()
        {
            _configurations.Add(new DbConfigurationModel
            {
                Uid = "ACFA.1",
                Type = "SPHINX_SRV",
                Properties = new List<Property>
                {
                    new Property { Id = "login", ValueString = "Administrator" },
                    new Property { Id = "password", ValueString = "12345" }
                }
            });
        }
        //добавить task async и cancellation token
        public Task<DbConfigurationModel[]> GetConfigurationsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_configurations.ToArray());
        }

        public Task<DbConfigurationModel> GetConfigurationAsync(string uid, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var configuration = _configurations.FirstOrDefault(x => x.Uid == uid);
            return Task.FromResult(configuration);
        }

        public Task SaveConfigurationAsync(DbConfigurationModel configuration, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _configurations.Add(configuration);
            return Task.CompletedTask;
        }
    }
}
