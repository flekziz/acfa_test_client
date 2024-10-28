using src.app.module.Models;
using System.Collections.Generic;
using System.Linq;
using src.app.module.Repositories;

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

        public IEnumerable<DbConfigurationModel> GetAllConfigurations()
        {
            return _configurations;
        }

        public DbConfigurationModel GetConfiguration(string uid)
        {
            return _configurations.FirstOrDefault(c => c.Uid == uid);
        }

        public void SaveConfiguration(DbConfigurationModel configuration)
        {
            _configurations.Add(configuration);
        }
    }
}
