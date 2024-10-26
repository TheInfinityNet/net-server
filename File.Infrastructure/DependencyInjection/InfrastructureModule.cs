using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.File.Infrastructure.Repositories;
using InfinityNetServer.Services.File.Infrastructure.Data;

namespace InfinityNetServer.Services.File.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<FileDbContext>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddScoped<IPostTagRepository, PostTagRepository>();
        services.AddScoped<ICommentTagRepository, CommentTagRepository>();
    }

}
