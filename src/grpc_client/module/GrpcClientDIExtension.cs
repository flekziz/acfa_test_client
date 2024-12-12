using grpc_client.module.Mapper;
using grpc_client.module.Abstractions;
using grpc_client.module.Implementations;

namespace Microsoft.Extensions.DependencyInjection;

public static class GrpcClientDIExtension
{
    public static void AddAcfaGrpcService(this IServiceCollection services)
    {
        services.AddSingleton<IGrpcService, GrpcService>();

        services.AddTransient<ManualMapper>();
        new MapBuilder()
            .Register(typeof(ModelToProtoMaps))
            .Register(typeof(ProtoToModelMaps));
        //ProtoToModelMaps.RegisterMaps(new MapBuilder());
        //ModelToProtoMaps.RegisterMaps(new MapBuilder());
    }
}
