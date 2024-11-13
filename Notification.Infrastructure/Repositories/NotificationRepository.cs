using InfinityNetServer.BuildingBlocks.Infrastructure.MongoDB.Repositories;
using InfinityNetServer.Services.Notification.Domain.Repositories;
using InfinityNetServer.Services.Notification.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Notification.Infrastructure.Repositories
{
    public class NotificationRepository(NotificationDbContext dbContext)
        : MongoDbGenericRepository<Domain.Entities.Notification, Guid>(dbContext), INotificationRepository
    {



    }
}
