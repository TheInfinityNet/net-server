using InfinityNetServer.Services.Notification.Domain.Repositories;
using InfinityNetServer.Services.Notification.Infrastructure.MongoDb;

namespace InfinityNetServer.Services.Notification.Infrastructure.Repositories
{
    public class NotificationRepository(NotificationDbContext dbContext)
        : MongoDbGenericRepository<Domain.Entities.Notification>(dbContext), INotificationRepository
    {



    }
}
