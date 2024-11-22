using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Profile.Application.GrpcServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace InfinityNetServer.Services.File.Presentation.Configurations;

public static class GrpcExtensions
{
    public static void MapGrpcServices(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGrpcService<GrpcFileService>();
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

        services.AddGrpcClient<PostService.PostServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:PostService"]);
        });

        services.AddGrpcClient<CommentService.CommentServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcServers:CommentService"]);
        });

        services.AddScoped(typeof(CommonIdentityClient));

        services.AddScoped(typeof(CommonProfileClient));

        services.AddScoped(typeof(CommonPostClient));

        services.AddScoped(typeof(CommonCommentClient));
    }

}
