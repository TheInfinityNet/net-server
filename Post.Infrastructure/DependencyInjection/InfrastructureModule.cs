using Microsoft.Extensions.DependencyInjection;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Data;
using InfinityNetServer.Services.Post.Domain.Repositories;
using InfinityNetServer.Services.Post.Infrastructure.Repositories;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;
using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories;

namespace InfinityNetServer.Services.Post.Infrastructure.DependencyInjection;

public static class InfrastructureModule
{

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostDbContext>();
        services.AddScoped<TimelineDbContext>().AddOptions<IConfiguration>().Bind(configuration);
        var discriminatorConvention = BsonSerializer.LookupDiscriminatorConvention(typeof(object));
        var objectSerializer = new ObjectSerializer(discriminatorConvention, GuidRepresentation.Standard);
        BsonSerializer.RegisterSerializer(objectSerializer);
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostPrivacyRepository, PostPrivacyRepository>();
        services.AddScoped<IPostPrivacyIncludeRepository, PostPrivacyIncludeRepository>();
        services.AddScoped<IPostPrivacyExcludeRepository, PostPrivacyExcludeRepository>();

        services.AddScoped(typeof(IMongoDbGenericRepository<,>), typeof(MongoDbGenericRepository<,>));
        services.AddScoped<IUserTimelineRepository, UserTimelineRepository>();
    }

}
