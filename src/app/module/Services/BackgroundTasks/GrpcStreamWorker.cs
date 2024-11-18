using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace app.module.Services.BackgroungTasks
{
    public class GrpcStreamWorker : BackgroundService
    {
        //private readonly GrpcClient _grpcClient; //grpc клиент 

        //public GrpcStreamWorker(GrpcClient _grpcClient)
        //{
        //    _grpcClient = grpcClient;
        //}
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //throw new System.NotImplementedException();
            return Task.CompletedTask;
        }
    }
}

