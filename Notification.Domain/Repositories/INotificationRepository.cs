using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using System;

namespace InfinityNetServer.Services.Notification.Domain.Repositories
{
    public interface INotificationRepository : IMongoDbGenericRepository<Entities.Notification, Guid>
    {



    }
}
