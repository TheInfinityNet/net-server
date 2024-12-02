using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.BuildingBlocks.Presentation.Exceptions;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.Grpc;

public static class GrpcConfiguration
{

    public static void AddGrpcPreConfigured(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcGlobalExceptionHandler>();
        });
    }
}
