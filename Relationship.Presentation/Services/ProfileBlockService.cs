using InfinityNetServer.Services.Relationship.Application;
using InfinityNetServer.Services.Relationship.Application.Services;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
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

    }

}

