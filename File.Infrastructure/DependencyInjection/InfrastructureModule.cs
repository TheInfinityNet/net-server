using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using InfinityNetServer.Services.File.Infrastructure.MongoDb;

namespace InfinityNetServer.Services.File.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<FileDbContext>().AddOptions<IConfiguration>().Bind(configuration);
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPhotoMetadataRepository, PhotoMetadataRepository>();
    }

}
