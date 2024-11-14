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
        CommonPostClient postClient,
        CommonCommentClient commentClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.CommentNotificationCommand>
    {

        public async Task Handle(DomainCommand.CommentNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create comment notification command handler");
            var profile = await profileClient.GetProfile(request.RelatedProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);
            var comment = await commentClient.GetPreviewComment(request.CommentId.ToString());
            var post = await postClient.GetPreviewPost(comment.PostId.ToString());
            switch (request.Type)
            {
                case BuildingBlocks.Domain.Enums.NotificationType.TaggedInComment:
                    await notificationService.Create(new Domain.Entities.Notification
                    {
                        AccountId = profile.AccountId,
                        ThumbnailId = !profile.Avatar.Id.Equals(Guid.Empty.ToString()) ? profile.Avatar.Id : null,
                        EntityId = request.CommentId.ToString(),
                        Type = BuildingBlocks.Domain.Enums.NotificationType.TaggedInComment,
                        Title = post.PreviewContent,
                        Content = triggerProfile.Name,
                        CreatedAt = request.CreatedAt
                    });
                    break;

                case BuildingBlocks.Domain.Enums.NotificationType.ReplyToComment:
                    await notificationService.Create(new Domain.Entities.Notification
                    {
                        AccountId = profile.AccountId,
                        ThumbnailId = !profile.Avatar.Id.Equals(Guid.Empty.ToString()) ? profile.Avatar.Id : null,
                        EntityId = request.CommentId.ToString(),
                        Type = BuildingBlocks.Domain.Enums.NotificationType.ReplyToComment,
                        Title = post.PreviewContent,
                        Content = triggerProfile.Name,
                        CreatedAt = request.CreatedAt
                    });
                    break;
            }
        }

    }
}
