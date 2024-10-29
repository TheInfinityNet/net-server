using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.Gateways.OcelotGateway.Presentaition.Configurations;

public static class GrpcExtensions
{

    public static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<IdentityService.IdentityServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:IdentityService"]!);
        });

        services.AddScoped(typeof(CommonIdentityClient));
    }

}
