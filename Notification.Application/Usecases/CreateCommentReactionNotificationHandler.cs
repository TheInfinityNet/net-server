using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Notification.Application.IServices;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Usecases
{
    public class CreateCommentReactionNotificationHandler
        (ILogger<CreateCommentReactionNotificationHandler> logger,
        CommonProfileClient profileClient,
        CommonCommentClient commentClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.CreateCommentReactionNotificationCommand>
    {
        public async Task Handle(DomainCommand.CreateCommentReactionNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create commnet reaction notification command handler");
            var profile = await profileClient.GetProfile(request.TargetProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);
            var comment = await commentClient.GetPreviewComment(request.CommentId.ToString());

            var notification = new Domain.Entities.Notification
            {
                AccountId = profile.AccountId,
                ThumbnailId = !triggerProfile.Avatar.Id.Equals(Guid.Empty.ToString()) ? triggerProfile.Avatar.Id : null,
                EntityId = request.CommentReactionId.ToString(),
                Type = request.Type,
                Permalink = "https://localhost:61000/comments/api/comments/preview-comment/" + request.CommentId.ToString(),
                TitleParams = [comment.PreviewContent],
                ContentParams = [triggerProfile.Name, request.ReactionType.ToString()],
                CreatedAt = request.CreatedAt,
            };
            await notificationService.Create(notification);
        }
    }
}
