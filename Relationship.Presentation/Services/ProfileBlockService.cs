using InfinityNetServer.Services.Relationship.Application;
using InfinityNetServer.Services.Relationship.Application.Services;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Presentation.Services
{
    public class ProfileBlockService(
    IProfileBlockRepository profileBlockRepository,
    ILogger<ProfileBlockService> logger,
    IStringLocalizer<RelationshipSharedResource> localizer) : IProfileBlockService
    {

        public async Task<bool> HasBlocked(string currentProfileId, string targetProfileId)
            => await profileBlockRepository.GetByBlockerIdAndBlockeeId(Guid.Parse(currentProfileId), Guid.Parse(targetProfileId)) != null;

        public async Task<IList<string>> GetBlockerIds(string profileId, int? limit)
        {
            var blockerIds = await profileBlockRepository.GetAllBlockerIdsAsync(Guid.Parse(profileId), limit);
            return blockerIds.Select(i => i.ToString()).ToList();
        }

        public async Task<IList<string>> GetBlockeeIds(string profileId, int? limit)
        {
            var blockerIds = await profileBlockRepository.GetAllBlockeeIdsAsync(Guid.Parse(profileId), limit);
            return blockerIds.Select(i => i.ToString()).ToList();
        }

    }

}

