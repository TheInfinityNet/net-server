using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Notification.Application.IServices;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.Usecases
{
    public class CreatePostReactionNotificationHandler
        (ILogger<CreatePostReactionNotificationHandler> logger,
        CommonProfileClient profileClient,
        CommonPostClient postClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.CreatePostReactionNotificationCommand>
    {
        public async Task Handle(DomainCommand.CreatePostReactionNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create post reaction notification command handler");
            var profile = await profileClient.GetProfile(request.TargetProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);
            var post = await postClient.GetPreviewPost(request.PostId.ToString());

            var notification = new Domain.Entities.Notification
            {
                AccountId = profile.AccountId,
                ThumbnailId = triggerProfile.Avatar?.Id,
                EntityId = request.PostReactionId.ToString(),
                Type = request.Type,
                Permalink = "https://localhost:61000/posts/get-post/" + request.PostId.ToString(),
                TitleParams = [post.PreviewContent],
                ContentParams = [triggerProfile.Name, request.ReactionType.ToString()],
                CreatedAt = request.CreatedAt,
            };
            await notificationService.Create(notification);
        }
    }
}
