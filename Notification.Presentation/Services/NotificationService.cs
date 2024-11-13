using InfinityNetServer.Services.Notification.Application.Services;
using InfinityNetServer.Services.Notification.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Presentation.Services
{
    public class NotificationService
        (ILogger<NotificationService> logger, INotificationRepository notificationRepository) : INotificationService
    {
        public async Task Create(Domain.Entities.Notification notification)
            => await notificationRepository.CreateAsync(notification);

        public async Task Delete(string id)
            => await notificationRepository.DeleteAsync(Guid.Parse(id));
        public Task<Domain.Entities.Notification> GetById(string id)
            => notificationRepository.GetByIdAsync(Guid.Parse(id));

        public async Task Update(Domain.Entities.Notification notification)
            => await notificationRepository.UpdateAsync(notification);

    }
}
