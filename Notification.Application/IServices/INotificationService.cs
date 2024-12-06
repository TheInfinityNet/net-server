using Google.Protobuf.WellKnownTypes;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.IServices
{
    public interface INotificationService
    {

        Task Create(Domain.Entities.Notification notification);

        Task Update(Domain.Entities.Notification notification);

        Task Delete(string id);

        Task<Domain.Entities.Notification> GetById(string id);

        Task<CursorPagedResult<Domain.Entities.Notification>> GetNewestNotifications(string accountId, string cursor, int pageSize = 10);

        Task<CursorPagedResult<Domain.Entities.Notification>> GetNewestUnreadOrMentionNotifications(string accountId, string cursor, int pageSize = 10, string type = "All");


        Task<Domain.Entities.Notification> ChangeReadStatusNotification(Guid notificationId);

        Task<Domain.Entities.Notification> RemoveNotification(Guid notificationId);
    }
}
