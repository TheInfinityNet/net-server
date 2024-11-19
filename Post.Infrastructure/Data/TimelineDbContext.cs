using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Post.Infrastructure.Data
{
    public class TimelineDbContext(IConfiguration configuration) : MongoDbContext(configuration)
    {



    }
}
