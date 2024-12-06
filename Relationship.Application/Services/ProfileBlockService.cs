using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Application.IServices;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public class ProfileBlockService(
    IProfileBlockRepository profileBlockRepository,
    ILogger<ProfileBlockService> logger,
    IStringLocalizer<RelationshipSharedResource> localizer) : IProfileBlockService
    {

        public async Task<bool> HasBlocked(string currentProfileId, string targetProfileId)
            => await profileBlockRepository.GetByBlockerIdAndBlockeeId(Guid.Parse(currentProfileId), Guid.Parse(targetProfileId)) != null;

        public async Task<IList<string>> GetAllBlockerIds(string profileId)
        {
            var blockerIds = await profileBlockRepository.GetAllBlockerIdsAsync(Guid.Parse(profileId));
            return blockerIds.Select(i => i.ToString()).ToList();
        }

        public async Task<IList<string>> GetAllBlockeeIds(string profileId)
        {
            var blockerIds = await profileBlockRepository.GetAllBlockeeIdsAsync(Guid.Parse(profileId));
            return blockerIds.Select(i => i.ToString()).ToList();
        }

        public async Task<ProfileBlock> GetByBlockerIdAndBlockeeIdAsync(string followerId, string followeeId)
            => await profileBlockRepository.GetByBlockerIdAndBlockeeId(Guid.Parse(followerId), Guid.Parse(followeeId));

        public async Task<CursorPagedResult<ProfileBlock>> GetBlockedList(string profileId, string cursor, int limit)
        {
            IList<string> blockeeIds = await GetAllBlockeeIds(profileId);

            var specification = new SpecificationWithCursor<ProfileBlock>
            {
                Criteria = profileBlock => blockeeIds.Contains(profileBlock.BlockeeId.ToString()),
                OrderFields = [
                        new OrderField<ProfileBlock>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                Limit = limit
            };
            return await profileBlockRepository.GetPagedAsync(specification);
        }

        public async Task<ProfileBlock> Block(string followerId, string followeeId)
            => await profileBlockRepository.CreateAsync(new ProfileBlock
            {
                BlockerId = Guid.Parse(followerId),
                BlockeeId = Guid.Parse(followeeId)
            });

        public async Task<UnBlockResponse> UnBlock(string blockId)
        {
            await profileBlockRepository.DeleteAsync(Guid.Parse(blockId));
            return new UnBlockResponse
            {
                Message = "UnBlocked",
                Status = "UnBlocked",
                UserId = blockId
            };
        }

    }

}

