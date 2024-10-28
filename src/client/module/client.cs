using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Drivers.Acfa.V2;
using Grpc.Core;
using Grpc.Net.Client;

namespace src.client;

internal class AcfaClient
{
    Drivers.Acfa.V2.ACFADriver.ACFADriverClient _client;
    internal AcfaClient(){
        var channel = GrpcChannel.ForAddress("http://localhost:5001");
        _client = new(channel);
    }

    internal async IAsyncEnumerable<ModuleMessage> Process(string lang, CancellationToken token)
    {
        var stream = _client.InitializeModule(new()
        {
            LocalizationLanguage = "ru"
        });

        await foreach (var message in stream.ResponseStream.ReadAllAsync(token))
        {
            yield return message;
        }
    }


    internal async Task CreateDevice(object device, CancellationToken token)
    {
        var req = new CreateDeviceRequest();

        await _client.CreateDeviceAsync(req);
    }

}
