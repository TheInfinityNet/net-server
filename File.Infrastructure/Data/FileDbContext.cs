using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.File.Infrastructure.Data
{
    public class FileDbContext(IConfiguration configuration) : MongoDbContext(configuration)
    {

        

    }
}
