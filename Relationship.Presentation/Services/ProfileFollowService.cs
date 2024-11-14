using InfinityNetServer.Services.Relationship.Application;
using InfinityNetServer.Services.Relationship.Application.Services;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace InfinityNetServer.Services.Relationship.Presentation.Services
{
    public class ProfileFollowService(
    IProfileFollowRepository profileFollowRepository,
    ILogger<ProfileFollowService> logger,
    IStringLocalizer<RelationshipSharedResource> localizer) : IProfileFollowService
    {

        public async Task<bool> HasFollowed(string currentProfileId, string targetProfileId)
            => await profileFollowRepository.GetByFollowerIdAndFolloweeIdAsync(Guid.Parse(currentProfileId), Guid.Parse(targetProfileId)) != null;

        public async Task<IList<string>> GetAllFolloweeIds(string currentProfileId, int? limit)
        {
            var followeeIds = await profileFollowRepository.GetAllFolloweeIdsAsync(Guid.Parse(currentProfileId), limit);
            return followeeIds.Select(i => i.ToString()).ToList();
        }

        public async Task<IList<string>> GetAllFollowerIds(string currentProfileId, int? limit)
        {
            var followerIds = await profileFollowRepository.GetAllFollowerIdsAsync(Guid.Parse(currentProfileId), limit);
            return followerIds.Select(i => i.ToString()).ToList();
        }

    }

}

