using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Notification.Application.Services;
using InfinityNetServer.Services.Notification.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Presentation.Services
{
    public class NotificationService
        (ILogger<NotificationService> logger, 
        INotificationRepository notificationRepository) : INotificationService
    {
        public async Task Create(Domain.Entities.Notification notification)
            => await notificationRepository.CreateAsync(notification);

        public async Task Delete(string id)
            => await notificationRepository.DeleteAsync(Guid.Parse(id));
        public Task<Domain.Entities.Notification> GetById(string id)
            => notificationRepository.GetByIdAsync(Guid.Parse(id));

        public async Task Update(Domain.Entities.Notification notification)
            => await notificationRepository.UpdateAsync(notification);

        public Task<CursorPagedResult<Domain.Entities.Notification>> GetNewestNotifications(string accountId, string? cursor, int pageSize)
        {
            var specification = new SpecificationWithCursor<Domain.Entities.Notification>
            {
                Criteria = x => x.AccountId == Guid.Parse(accountId), // where account id = {id}
                OrderFields = [
                        new OrderField<Domain.Entities.Notification>
                        {
                            Field = x => x.IsRead,
                            Direction = SortDirection.Ascending
                        },
                        new OrderField<Domain.Entities.Notification>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                PageSize = 10
            };

            return notificationRepository.GetPagedDataAsync(cursor, specification);
        }
    }
}
