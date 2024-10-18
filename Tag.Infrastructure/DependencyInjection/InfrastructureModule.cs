using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Tag.Infrastructure.Data;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Infrastructure.Repositories;

namespace InfinityNetServer.Services.Tag.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<TagDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddScoped<IPostTagRepository, PostTagRepository>();
        services.AddScoped<ICommentTagRepository, CommentTagRepository>();
    }

}
