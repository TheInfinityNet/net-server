using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.Usecases
{
    public class UpdateUserTimelineHandler
        (ILogger<UpdateUserTimelineHandler> logger,
        IUserTimelineService userTimelineService) : IRequestHandler<DomainCommand.UpdateUserTimelineCommand>
    {

        public async Task Handle(DomainCommand.UpdateUserTimelineCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting create post timeline command handler");

            await userTimelineService.CreateIfNotExists(request.ProfileId.ToString());

            var postTimeline = new TimelinePost
            {
                PostId = request.PostId,
                CreatedAt = request.CreatedAt
            };

            await userTimelineService.UpdateUserTimeline(request.ProfileId.ToString(), postTimeline);
        }

    }
}
