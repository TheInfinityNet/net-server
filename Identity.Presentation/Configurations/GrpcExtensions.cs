using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Identity.Application.GrpcClients;
using InfinityNetServer.Services.Identity.Application.GrpcServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.Services.Identity.Presentation.Configurations;

public static class GrpcExtensions
{
    public static void MapGrpcServices(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<GrpcIdentityService>();
    }

    public static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:Profile-Service"]);
        });

        services.AddScoped(typeof(ProfileClient));
    }

}
