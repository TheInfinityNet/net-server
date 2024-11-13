using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories;
using InfinityNetServer.Services.Notification.Domain.Repositories;
using InfinityNetServer.Services.Notification.Infrastructure.Data;
using InfinityNetServer.Services.Notification.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfinityNetServer.Services.Notification.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<NotificationDbContext>().AddOptions<IConfiguration>().Bind(configuration);
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IMongoDbGenericRepository<,>), typeof(MongoDbGenericRepository<,>));
        services.AddScoped<INotificationRepository, NotificationRepository>();
    }

}
