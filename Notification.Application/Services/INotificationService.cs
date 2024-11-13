using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Services
{
    public interface INotificationService
    {

        Task Create(Domain.Entities.Notification notification);

        Task Update(Domain.Entities.Notification notification);

        Task Delete(string id);

        Task<Domain.Entities.Notification> GetById(string id);

    }
}
