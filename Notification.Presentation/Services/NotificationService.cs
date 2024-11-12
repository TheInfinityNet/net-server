using InfinityNetServer.Services.Notification.Application.Services;
using InfinityNetServer.Services.Notification.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Presentation.Services
{
    public class NotificationService
        (ILogger<NotificationService> logger, INotificationRepository photoMetadataRepository) : INotificationService
    {
        public async Task Create(Domain.Entities.Notification notification)
            => await photoMetadataRepository.CreateAsync(notification);

        public async Task Delete(string id)
        {
            Domain.Entities.Notification notification = await GetById(id);
            await photoMetadataRepository.DeleteAsync(notification);
        }

        public Task<Domain.Entities.Notification> GetById(string id)
            => photoMetadataRepository.GetByIdAsync(Guid.Parse(id));

        public async Task Update(Domain.Entities.Notification notification)
            => await photoMetadataRepository.UpdateAsync(notification);

    }
}
