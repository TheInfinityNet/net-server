using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Services
{
    public interface INotificationService
    {

        Task Create(Domain.Entities.Notification notification);

        Task Update(Domain.Entities.Notification notification);

        Task Delete(string id);

        Task<Domain.Entities.Notification> GetById(string id);

        Task<BCursorPagedResult<Domain.Entities.Notification>> GetNewestNotifications(string accountId, string? cursor, int pageSize = 10);

    }
}
