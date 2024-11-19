using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Data;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using InfinityNetServer.Services.Relationship.Infrastructure.Repositories;

namespace InfinityNetServer.Services.Relationship.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<RelationshipDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<IProfileFollowRepository, ProfileFollowRepository>();
        services.AddScoped<IProfileBlockRepository, ProfileBlockRepository>();
    }

}
