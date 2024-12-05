using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using InfinityNetServer.Services.Relationship.Application.GrpcClients;
using InfinityNetServer.Services.Relationship.Application.GrpcServices;

namespace InfinityNetServer.Services.Relationship.Presentation.Configurations;

public static class GrpcExtensions
{
    public static void MapGrpcServices(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<GrpcRelationshipService>();
    }

    public static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<IdentityService.IdentityServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:IdentityService"]!);
        });

        services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:ProfileService"]!);
        });

        services.AddGrpcClient<FileService.FileServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:FileService"]);
        });

        services.AddScoped(typeof(CommonIdentityClient));

        services.AddScoped(typeof(CommonProfileClient));

        services.AddScoped(typeof(ProfileClient));

        services.AddScoped(typeof(CommonFileClient));
    }

}
