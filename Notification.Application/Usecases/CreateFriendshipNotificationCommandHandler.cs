using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Notification.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Usecases
{
    public class CreateFriendshipNotificationCommandHandler
        (ILogger<CreateFriendshipNotificationCommandHandler> logger,
        CommonProfileClient profileClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.FriendshipNotificationCommand>
    {

        public async Task Handle(DomainCommand.FriendshipNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create friendship notification command handler");
            var profile = await profileClient.GetProfile(request.RelatedProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);
            switch (request.Type)
            {
                case BuildingBlocks.Domain.Enums.NotificationType.FriendInvitation:
                    await notificationService.Create(new Domain.Entities.Notification
                    {
                        AccountId = profile.AccountId,
                        ThumbnailId = !profile.Avatar.Id.Equals(Guid.Empty.ToString()) ? profile.Avatar.Id : null,
                        EntityId = request.FriendshipId.ToString(),
                        Type = BuildingBlocks.Domain.Enums.NotificationType.FriendInvitation,
                        Title = "FriendInvitationNotification.Title",
                        Content = triggerProfile.Name,
                        CreatedAt = request.CreatedAt
                    });
                    break;
            }
        }

    }
}
