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
    public class CreatePostNotificationHandler
        (ILogger<CreatePostNotificationHandler> logger,
        CommonProfileClient profileClient,
        CommonPostClient postClient,
        INotificationService notificationService) : IRequestHandler<DomainCommand.CreatePostNotificationCommand>
    {

        public async Task Handle(DomainCommand.CreatePostNotificationCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create post notification command handler");
            var profile = await profileClient.GetProfile(request.TargetProfileId.ToString());
            var triggerProfile = await profileClient.GetProfile(request.TriggeredBy);
            var post = await postClient.GetPreviewPost(request.PostId.ToString());

            var notification = new Domain.Entities.Notification
            {
                AccountId = profile.AccountId,
                ThumbnailId = !profile.Avatar.Id.Equals(Guid.Empty.ToString()) ? profile.Avatar.Id : null,
                EntityId = request.PostId.ToString(),
                Type = request.Type,
                Permalink = "https://localhost:61000/posts/get-post/" + request.PostId.ToString(),
                TitleParams = [post.PreviewContent],
                ContentParams = [triggerProfile.Name],
                CreatedAt = request.CreatedAt
            };

            await notificationService.Create(notification);
        }

    }
}
