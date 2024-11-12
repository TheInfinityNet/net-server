using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.Services.Notification.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Usecases
{
    public class CreatePostNotificationCommandHandler
        (ILogger<CreatePostNotificationCommandHandler> logger,
        INotificationService notificationService) : IRequestHandler<DomainCommand.PostNotificationCommand>
    {

        public async Task Handle(DomainCommand.PostNotificationCommand request, CancellationToken cancellationToken)
        {
            
        }

    }
}
