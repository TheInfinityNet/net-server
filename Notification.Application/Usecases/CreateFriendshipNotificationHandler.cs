using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Notification.Application.IServices;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Usecases
{
    public class CreateFriendshipNotificationHandler
        (ILogger<CreateFriendshipNotificationHandler> logger,
        CommonProfileClient profileClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.CreateFriendshipNotificationCommand>
    {

        public async Task Handle(DomainCommand.CreateFriendshipNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create friendship notification command handler");
            var profile = await profileClient.GetProfile(request.TargetProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);

            var notification = new Domain.Entities.Notification
            {
                AccountId = profile.AccountId,
                ThumbnailId = triggerProfile.Avatar?.Id,
                EntityId = request.FriendshipId.ToString(),
                Type = request.Type,
                Permalink = "https://localhost:61000/profiles/users/" + request.TriggeredBy.ToString(),
                TitleParams = [],
                ContentParams = [triggerProfile.Name],
                CreatedAt = request.CreatedAt
            };

            await notificationService.Create(notification);
        }

    }
}
