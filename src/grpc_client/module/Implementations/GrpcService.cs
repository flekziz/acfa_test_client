using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using grpc_client.module.Mapper;
using grpc_client.module.Models;
using grpc_client.module.Abstractions;
using grpc_client.module.Configuration;
using Microsoft.Extensions.Logging;

namespace grpc_client.module.Implementations;

public class GrpcService : IGrpcService
{
    GrpcClient? grpcClient;
    ManualMapper _mapper;
    CancellationTokenSource _eventsCts;
    ILogger<GrpcService> _logger;
    public GrpcService(ManualMapper mapper, ILogger<GrpcService> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _eventsCts = new CancellationTokenSource();
    }

    public void StartConnection(Action<GrpcConfigurationBuilder>? builderMethod = null)
    {
        var builder = new GrpcConfigurationBuilder();
        if (builderMethod != null)
            builderMethod(builder);
        var conf = builder.GetConfiguration();
        if (grpcClient != null)
            DropConnection();
        grpcClient = new GrpcClient(conf, _mapper);
        _logger.LogInformation("Grpc client connected");
    }

    public void DropConnection()
    {
        if (grpcClient == null)
            return;
        _eventsCts.Cancel();
        _eventsCts = new CancellationTokenSource();

        grpcClient.StopEventsProcessing();

        // maybe a proper Dispose()? meh, garbage collector exists for a reason
        grpcClient = null;
        _logger.LogInformation("Dropped grpc client connection");
    }

    public async IAsyncEnumerable<ModuleEvent> GetEvents(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting getting events stream");
        if (grpcClient == null) yield break;
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _eventsCts.Token))
        {
            await foreach(var e in grpcClient.ProcessEvents(linkedCts.Token))
                yield return e;
        }
        _logger.LogInformation("Events stream ended");
        DropConnection();
    }

    public async Task CreateDevice(Device device, Unit configuration, CancellationToken token = default) =>
        await WithCheck((client) => client.CreateDevice(device, configuration, token));


    public async Task<List<Unit>> ListUnit(string uid, int displayMode, CancellationToken token = default) =>
        await ListUnits([uid], displayMode, token);
    public async Task<List<Unit>> ListUnits(ICollection<string> uids, int displayMode, CancellationToken token = default) =>
        await WithCheck((client) => client.ListUnits(uids, displayMode, token));


    public async Task<List<UnitEvents>> ListUnitEvents(string uid, CancellationToken token = default) =>
        await ListUnitsEvents([uid], token);
    public async Task<List<UnitEvents>> ListUnitsEvents(ICollection<string> uids, CancellationToken token = default) =>
        await WithCheck((client) => client.ListUnitsEvents(uids, token));


    public async Task<Unit> DownloadConfiguration(string uid, CancellationToken token = default) =>
        await WithCheck((client) => client.DownloadConfiguration(uid, token));


    public async Task<bool> AddUnit(string parentUid, Unit unit, CancellationToken token = default) =>
        (await AddUnits(parentUid, [unit], token)).Ok();
    public async Task<ConfigurationChangeResponse> AddUnits(string parentUid, ICollection<Unit> units, CancellationToken token = default) =>
        await WithCheck((client) => client.AddUnits(parentUid, units, token));


    public async Task<bool> ChangeUnit(ChangeUnit changed, CancellationToken token = default) =>
        (await ChangeUnits([changed], token)).Ok();
    public async Task<ConfigurationChangeResponse> ChangeUnits(ICollection<ChangeUnit> changed, CancellationToken token = default) =>
        await WithCheck((client) => client.ChangeUnits(changed, token));


    public async Task<bool> RemoveUnit(RemoveUnit removed, CancellationToken token = default) =>
        (await RemoveUnits([removed], token)).Ok();
    public async Task<ConfigurationChangeResponse> RemoveUnits(ICollection<RemoveUnit> removed, CancellationToken token = default) =>
        await WithCheck((client) => client.RemoveUnits(removed, token));


    const string GrpcClientIsNullError = "GrpcClient is null and can't send a request. Perhaps it wasn't (re)connected";
    private async Task<T> WithCheck<T>(Func<GrpcClient, Task<T>> func)
    {
        if (grpcClient == null)
        {
            _logger.LogCritical(GrpcClientIsNullError);
            throw new Exception(GrpcClientIsNullError);
        }
        return await func(grpcClient);
    }
    private async Task WithCheck(Func<GrpcClient, Task> func)
    {
        if (grpcClient == null)
        {
            _logger.LogCritical(GrpcClientIsNullError);
            throw new Exception(GrpcClientIsNullError);
        }
        await func(grpcClient);
    }
}
