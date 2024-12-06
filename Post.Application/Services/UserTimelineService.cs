using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Post.Application.IServices;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Application.Services
{
    public class UserTimelineService(
        ILogger<UserTimelineService> logger,
        IUserTimelineRepository userTimelineRepository) : IUserTimelineService
    {
        public async Task<CursorPagedResult<TimelinePost>> GetUserTimeline(string profileId, string cursor, int pageSize)
            => await userTimelineRepository.GetUserTimelineAsync(Guid.Parse(profileId), cursor, pageSize);

        public async Task UpdateUserTimeline(string profileId, TimelinePost post)
            => await userTimelineRepository.PushPostToTimelineAsync(Guid.Parse(profileId), post);

        public async Task CreateIfNotExists(string profileId)
        {
            bool exists = await userTimelineRepository.IsExists(Guid.Parse(profileId));

            if (!exists)
            {
                var userTimeline = new UserTimeline
                {
                    ProfileId = Guid.Parse(profileId)
                };

                await userTimelineRepository.CreateAsync(userTimeline);
            }
        }
    }
}
