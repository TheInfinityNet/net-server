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
    public class CreatePostNotificationCommandHandler
        (ILogger<CreatePostNotificationCommandHandler> logger,
        CommonProfileClient profileClient,
        CommonPostClient postClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.PostNotificationCommand>
    {

        public async Task Handle(DomainCommand.PostNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create post notification command handler");
            var profile = await profileClient.GetProfile(request.RelatedProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);
            var post = await postClient.GetPreviewPost(request.PostId.ToString());
            switch (request.Type)
            {
                case BuildingBlocks.Domain.Enums.NotificationType.TaggedInPost:
                    await notificationService.Create(new Domain.Entities.Notification
                    {
                        AccountId = profile.AccountId,
                        ThumbnailId = profile.Avatar.Id,
                        EntityId = request.PostId.ToString(),
                        Type = BuildingBlocks.Domain.Enums.NotificationType.TaggedInComment,
                        Title = $"Post: {post.PreviewContent}",
                        Content = $"{triggerProfile.Name} tagged you in a post",
                        CreatedAt = request.CreatedAt
                    });
                    break;
            }
        }

    }
}
