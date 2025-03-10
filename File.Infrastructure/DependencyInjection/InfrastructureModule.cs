﻿using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.Data;
using InfinityNetServer.Services.File.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.File.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<FileDbContext>().AddOptions<IConfiguration>().Bind(configuration);
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IMongoDbGenericRepository<,>), typeof(MongoDbGenericRepository<,>));
        services.AddScoped<IPhotoMetadataRepository, PhotoMetadataRepository>();
        services.AddScoped<IVideoMetadataRepository, VideoMetadataRepository>();
    }

}
