using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.IServices
{
    public interface IProfileBlockService
    {

        public Task<bool> HasBlocked(string currentProfileId, string targetProfileId);

        public Task<IList<string>> GetAllBlockerIds(string profileId);

        public Task<IList<string>> GetAllBlockeeIds(string profileId);

        public Task<ProfileBlock> GetByBlockerIdAndBlockeeIdAsync(string followerId, string followeeId);

        public Task<CursorPagedResult<ProfileBlock>> GetBlockedList(string profileId, string cursor, int limit);

        public Task<ProfileBlock> Block(string followerId, string followeeId);

        public Task<UnBlockResponse> UnBlock(string blockId);

    }
}
