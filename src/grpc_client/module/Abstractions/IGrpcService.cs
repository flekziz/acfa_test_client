using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using grpc_client.module.Models;
using grpc_client.module.Configuration;

namespace grpc_client.module.Abstractions;

public interface IGrpcService
{
    public void StartConnection(Action<GrpcConfigurationBuilder>? builderMethod = null);
    public void DropConnection();
    public IAsyncEnumerable<ModuleEvent> GetEvents(CancellationToken token);

    public Task CreateDevice(Device device, Unit configuration, CancellationToken token = default);

    public Task<List<Unit>> ListUnit(string uid, int displayMode, CancellationToken token = default);
    public Task<List<Unit>> ListUnits(ICollection<string> uids, int displayMode, CancellationToken token = default);

    public Task<List<UnitEvents>> ListUnitEvents(string uid, CancellationToken token = default);
    public Task<List<UnitEvents>> ListUnitsEvents(ICollection<string> uids, CancellationToken token = default);
    
    public Task<Unit> DownloadConfiguration(string uid, CancellationToken token = default);

    public Task<bool> AddUnit(string parentUid, Unit unit, CancellationToken token = default);
    public Task<ConfigurationChangeResponse> AddUnits(string parentUid, ICollection<Unit> units, CancellationToken token = default);

    public Task<bool> ChangeUnit(ChangeUnit changed, CancellationToken token = default);
    public Task<ConfigurationChangeResponse> ChangeUnits(ICollection<ChangeUnit> changed, CancellationToken token = default);

    public Task<bool> RemoveUnit(RemoveUnit removed, CancellationToken token = default);
    public Task<ConfigurationChangeResponse> RemoveUnits(ICollection<RemoveUnit> removed, CancellationToken token = default);

}
