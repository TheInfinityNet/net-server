using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Notification.Application.IServices;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Usecases
{
    public class CreateCommentNotificationHandler
        (ILogger<CreateCommentNotificationHandler> logger,
        CommonProfileClient profileClient,
        CommonPostClient postClient,
        CommonCommentClient commentClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.CreateCommentNotificationCommand>
    {

        public async Task Handle(DomainCommand.CreateCommentNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create comment notification command handler");
            var profile = await profileClient.GetProfile(request.TargetProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);
            var comment = await commentClient.GetPreviewComment(request.CommentId.ToString());
            var post = await postClient.GetPreviewPost(comment.PostId.ToString());

            var notification = new Domain.Entities.Notification
            {
                AccountId = profile.AccountId,
                ThumbnailId = triggerProfile.Avatar?.Id,
                EntityId = request.CommentId.ToString(),
                Type = request.Type,
                Permalink = "https://localhost:61000/comments/api/comments/preview-comment/" + request.CommentId.ToString(),
                TitleParams = [post.PreviewContent],
                ContentParams = [triggerProfile.Name],
                CreatedAt = request.CreatedAt
            };
            await notificationService.Create(notification);
        }

    }
}
