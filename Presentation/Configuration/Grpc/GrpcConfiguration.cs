using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.BuildingBlocks.Presentation.Exceptions;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.Grpc;

public static class GrpcConfiguration
{

    public static void AddGrpcPreConfigured(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcGlobalExceptionHandler>();
        });
    }
}
