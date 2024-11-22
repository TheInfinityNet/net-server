using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Notification.Infrastructure.Data
{
    public class NotificationDbContext(IConfiguration configuration) : MongoDbContext(configuration)
    {



    }
}
