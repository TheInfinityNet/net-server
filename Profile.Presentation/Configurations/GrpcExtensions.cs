﻿using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Profile.Application.GrpcServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.Services.Profile.Presentation.Configurations;

public static class GrpcExtensions
{
    public static void MapGrpcServices(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<GrpcProfileService>();
    }

    public static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<IdentityService.IdentityServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:IdentityService"]!);
        });

        services.AddGrpcClient<RelationshipService.RelationshipServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:RelationshipService"]!);
        });

        services.AddGrpcClient<FileService.FileServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:FileService"]);
        });

        services.AddScoped(typeof(CommonIdentityClient));

        services.AddScoped(typeof(CommonRelationshipClient));

        services.AddScoped(typeof(CommonFileClient));

    }

}
