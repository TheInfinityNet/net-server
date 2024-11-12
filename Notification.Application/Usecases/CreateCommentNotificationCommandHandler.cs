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
    public class CreateCommentNotificationCommandHandler
        (ILogger<CreateCommentNotificationCommandHandler> logger,
        CommonProfileClient profileClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.CommentNotificationCommand>
    {

        public async Task Handle(DomainCommand.CommentNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create comment notification command handler");
            var profile = await profileClient.GetProfile(request.RelatedProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);
            switch (request.Type)
            {
                case BuildingBlocks.Domain.Enums.NotificationType.TaggedInComment:
                    await notificationService.Create(new Domain.Entities.Notification
                    {
                        AccountId = profile.AccountId,
                        ThumbnailId = !profile.Avatar.Id.Equals(string.Empty) ? Guid.Parse(profile.Avatar.Id) : null,
                        Permalink = $"/{request.CommentId}",
                        Type = BuildingBlocks.Domain.Enums.NotificationType.TaggedInComment,
                        Title = "Tagged in comment",
                        Content = $"{triggerProfile.Name} tagged you in a comment",
                    });
                    break;

                case BuildingBlocks.Domain.Enums.NotificationType.ReplyToComment:
                    await notificationService.Create(new Domain.Entities.Notification
                    {
                        AccountId = profile.AccountId,
                        ThumbnailId = !profile.Avatar.Id.Equals(string.Empty) ? Guid.Parse(profile.Avatar.Id) : null,
                        Permalink = $"/{request.CommentId}",
                        Type = BuildingBlocks.Domain.Enums.NotificationType.ReplyToComment,
                        Title = "Replied to comment",
                        Content = $"{triggerProfile.Name} replied to your comment",
                    });
                    break;
            }
        }

    }
}
