﻿using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.Services.Profile.Presentation.Configurations;

public static class GrpcExtensions
{
    public static void MapGrpcServices(this IEndpointRouteBuilder endpoints)
    {
        //endpoints.MapGrpcService<>();
    }

    public static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<IdentityService.IdentityServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:Identity-Service"]);
        });

        services.AddScoped(typeof(IdentityClient));
    }

}