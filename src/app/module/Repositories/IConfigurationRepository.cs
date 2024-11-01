using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using src.app.module.Models;

namespace src.app.module.Repositories
{
    //абстракция методов для работы с хранением конфигураций
    public interface IConfigurationRepository
    {
        Task<DbConfigurationModel> GetConfigurationAsync(string uid, CancellationToken cancellationToken);
        Task SaveConfigurationAsync(DbConfigurationModel configuration, CancellationToken cancellationToken);
        Task<DbConfigurationModel[]> GetConfigurationsAsync(CancellationToken cancellationToken);
    }
}
