using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Notification.Application.IServices;
using InfinityNetServer.Services.Notification.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Services
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

        public Task<CursorPagedResult<Domain.Entities.Notification>> GetNewestNotifications(string accountId, string cursor, int pageSize)
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
                Limit = pageSize
            };

            return notificationRepository.GetPagedDataAsync(cursor, specification);
        }

        public async Task<Domain.Entities.Notification> ChangeReadStatusNotification(Guid notificationId)
        {
            var exist = await notificationRepository.GetByIdAsync(notificationId);
            if (exist != null)
            {
                exist.IsRead = !exist.IsRead;
                await notificationRepository.UpdateAsync(exist);
            }
            return exist ?? null;
        }

        public async Task<Domain.Entities.Notification> RemoveNotification(Guid notificationId)
        {
            var exist = await notificationRepository.GetByIdAsync(notificationId);
            if (exist != null)
            {
                await notificationRepository.DeleteAsync(notificationId);
            }
            return exist ?? null;
        }

        public Task<CursorPagedResult<Domain.Entities.Notification>> GetNewestUnreadOrMentionNotifications(string accountId, string cursor, int pageSize = 10, string type = "All")
        {
            var specification = new SpecificationWithCursor<Domain.Entities.Notification> { };
            switch (type)
            {
                case "Unread":
                    {
                        specification = new SpecificationWithCursor<Domain.Entities.Notification>
                        {

                            Criteria = x => x.AccountId == Guid.Parse(accountId) && x.IsRead == false, // where account id = {id}
                            OrderFields = [
                                new OrderField<Domain.Entities.Notification>
                                {
                                    Field = x => x.IsRead,
                                    Direction = SortDirection.Ascending
                                }
                            ],
                            Cursor = cursor,
                            Limit = pageSize
                        };
                    }
                    break;

                case "Mention":
                    {
                        specification = new SpecificationWithCursor<Domain.Entities.Notification>
                        {

                            Criteria = x => x.AccountId == Guid.Parse(accountId) && (x.Type == NotificationType.TaggedInPost || x.Type == NotificationType.TaggedInComment), // where account id = {id}
                            OrderFields = [
                                new OrderField<Domain.Entities.Notification>
                                {
                                    Field = x => x.IsRead,
                                    Direction = SortDirection.Ascending
                                }
                            ],
                            Cursor = cursor,
                            Limit = pageSize
                        };
                    }
                    break;
                default:
                    {
                        specification = new SpecificationWithCursor<Domain.Entities.Notification>
                        {
                            Criteria = x => x.AccountId == Guid.Parse(accountId), // where account id = {id}
                            OrderFields = [
                                new OrderField<Domain.Entities.Notification>
                                {
                                    Field = x => x.IsRead,
                                    Direction = SortDirection.Ascending
                                }
                            ],
                            Cursor = cursor,
                            Limit = pageSize
                        };
                    }
                    break;
            }
            return notificationRepository.GetPagedDataAsync(cursor, specification);
        }
    }
}
