﻿using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Post.Application.GrpcServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.Services.Post.Presentation.Configurations;

public static class GrpcExtensions
{
    public static void MapGrpcServices(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<GrpcPostService>();
    }

    public static void AddGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<IdentityService.IdentityServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:IdentityService"]);
        });

        services.AddGrpcClient<ProfileService.ProfileServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:ProfileService"]);
        });

        services.AddGrpcClient<GroupService.GroupServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:GroupService"]);
        });

        services.AddGrpcClient<RelationshipService.RelationshipServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:RelationshipService"]);
        });

        services.AddGrpcClient<FileService.FileServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:FileService"]);
        });

        services.AddGrpcClient<CommentService.CommentServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:CommentService"]);
        });

        services.AddGrpcClient<ReactionService.ReactionServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:ReactionService"]);
        });

        services.AddScoped(typeof(CommonIdentityClient));

        services.AddScoped(typeof(CommonProfileClient));

        services.AddScoped(typeof(CommonGroupClient));

        services.AddScoped(typeof(CommonRelationshipClient));

        services.AddScoped(typeof(CommonFileClient));

        services.AddScoped(typeof(CommonCommentClient));

        services.AddScoped(typeof(CommonReactionClient));

    }

}
