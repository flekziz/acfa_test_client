using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace src.client;

internal class AcfaWorker : BackgroundService
{
    private readonly AcfaClient _client;

    public AcfaWorker(AcfaClient client, IOptions<object> clientOptions) =>
        _client = client;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //"-l ru / --lang ru"

        var args = Environment.GetCommandLineArgs();

        var lang = !args.Any(x => x.Contains("-l")) ? "ru" : "ru";//parse(args).Lang;

        await foreach (var item in _client.Process("ru", stoppingToken))
        {
            switch (item.Event.DataCase)
            {
                case Drivers.Acfa.V2.ModuleEvent.DataOneofCase.None:
                    break;
                case Drivers.Acfa.V2.ModuleEvent.DataOneofCase.Event:
                    var @event = item.Event.Event;
                    break;
                case Drivers.Acfa.V2.ModuleEvent.DataOneofCase.State:
                    var @state = item.Event.State;
                    break;
                case Drivers.Acfa.V2.ModuleEvent.DataOneofCase.Status:
                    break;
            }


            throw new System.NotImplementedException();
        }
    }
}
