using System.Collections.Generic;
using src.app.module.Models;

namespace src.app.module.Repositories
{
    //абстракция методов для работы с хранением конфигураций
    public interface IConfigurationRepository
    {
        DbConfigurationModel GetConfiguration(string uid);
        void SaveConfiguration(DbConfigurationModel configuration);
        IEnumerable<DbConfigurationModel> GetAllConfigurations();
    }
}
