using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Grpc.Core;
using Grpc.Net.Client;
using grpc_client.module.Configuration;
using grpc_client.module.Models;
using System.Linq;
using grpc_client.module.Mapper;
using System.Data;

namespace grpc_client.module.Implementations;
internal class GrpcClient
{
    Drivers.Acfa.V2.ACFADriver.ACFADriverClient _client;
    GrpcConfiguration _conf;
    ManualMapper _mapper;

    internal GrpcClient(GrpcConfiguration configuration, ManualMapper mapper)
    {
        _conf = configuration;
        var channel = GrpcChannel.ForAddress(_conf.endpoint);
        _client = new(channel);
        _mapper = mapper;
    }
    

    internal async IAsyncEnumerable<ModuleEvent> ProcessEvents(CancellationToken token = default)
    {
        var stream = _client.InitializeModule(new()
        {
            LocalizationLanguage = _conf.lang,

        }, cancellationToken: token);
        await foreach (var message in stream.ResponseStream.ReadAllAsync(cancellationToken: token))
        {
            if (message.ValueCase == Drivers.Acfa.V2.ModuleMessage.ValueOneofCase.Event)
                yield return _mapper.Map<ModuleEvent>(message.Event)!;
            
            if (message.ValueCase == Drivers.Acfa.V2.ModuleMessage.ValueOneofCase.EventPackage)
                foreach (var e in message.EventPackage.Events)
                    yield return _mapper.Map<ModuleEvent>(e)!;
        }
    }

    internal void StopEventsProcessing()
    {
        try
        {
            _client.TerminateModule(new Google.Protobuf.WellKnownTypes.Empty());
        }
        catch (Exception) { }
    }


    internal async Task CreateDevice(Device device, Unit configuration, CancellationToken token = default) =>
        await _client.CreateDeviceAsync(
            _mapper.Map<Drivers.Acfa.V2.CreateDeviceRequest>((device, configuration)),
            cancellationToken: token);

    internal async Task<List<Unit>> ListUnits(ICollection<string> uids, int displayMode, CancellationToken token = default)
    {
        Drivers.Acfa.V2.ListUnitsRequest req = new()
        {
            DisplayMode = displayMode
        };
        req.UnitUids.AddRange(uids);

        var resp = await _client.ListUnitsAsync(req, cancellationToken: token);
        if (resp.Units.Count > 0)
            return _mapper.MapEnumerable<Drivers.Acfa.V2.UnitDescriptor, Unit>(resp.Units).ToList();
        return new();
    }

    internal async Task<List<UnitEvents>> ListUnitsEvents(ICollection<string> uids, CancellationToken token = default)
    {
        Drivers.Acfa.V2.ListUnitsEventsRequest req = new();
        req.Units.AddRange(uids.Select(id => new Drivers.Acfa.V2.Unit() { Uid = id }));

        var resp = await _client.ListUnitsEvents2Async(req, cancellationToken: token);
        if (resp.Items.Count > 0)
            return _mapper.MapEnumerable<Drivers.Acfa.V2.ListUnitsEventsResponse.Types.UnitEvents, UnitEvents>(resp.Items).ToList();
        return new();
    }


    internal async Task<Unit> DownloadConfiguration(string uid, CancellationToken token = default)
    {
        Drivers.Acfa.V2.DownloadConfigurationRequest req = new() { Uid = uid };
        var stream = _client.DownloadConfiguration(req, cancellationToken: token);
        Dictionary<string, Unit> units = new();
        await foreach (var item in stream.ResponseStream.ReadAllAsync())
        {
            var conf = _mapper.Map<DownloadConfigurationResponse>(item)!;
            units.Add(conf.ParentUid, conf.Unit);
        }
        // now turning it into a tree structure
        foreach (var t in units)
        {
            var u = units.Values.FirstOrDefault(u => u.Uid == t.Key);
            if (u == null)
                continue;
            u.Configurations.Add(t.Value);
        }
        Unit ret = units.Values.First(u => u.Uid == uid);
        return ret;
    }

    internal async Task<ConfigurationChangeResponse> AddUnits(string parentUid, ICollection<Unit> units, CancellationToken token = default)
    {
        Drivers.Acfa.V2.AddUnitsRequest req = new();
        req.ParentUid = parentUid;
        req.Units.AddRange(
            _mapper.MapEnumerable<Unit, Drivers.Acfa.V2.UnitDescriptor>(units));

        return _mapper.Map<ConfigurationChangeResponse>(
            await _client.AddUnitsAsync(req, cancellationToken: token))!;
    }

    internal async Task<ConfigurationChangeResponse> ChangeUnits(ICollection<ChangeUnit> changed, CancellationToken token = default)
    {
        Drivers.Acfa.V2.ChangeUnitsRequest req = new();
        req.Changed.AddRange(
            _mapper.MapEnumerable<ChangeUnit, 
                Drivers.Acfa.V2.ChangeUnitsRequest.Types.ChangeUnit>(changed));

        return _mapper.Map<ConfigurationChangeResponse>(
            await _client.ChangeUnitsAsync(req, cancellationToken: token))!;
    }

    internal async Task<ConfigurationChangeResponse> RemoveUnits(ICollection<RemoveUnit> removed, CancellationToken token = default)
    {
        Drivers.Acfa.V2.RemoveUnitsRequest req = new();
        req.Removed.AddRange(
            _mapper.MapEnumerable<RemoveUnit,
                Drivers.Acfa.V2.RemoveUnitsRequest.Types.RemoveUnit>(removed));

        return _mapper.Map<ConfigurationChangeResponse>(
            await _client.RemoveUnitsAsync(req, cancellationToken: token))!;
    }

}
