using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.Services
{
    public interface IProfileBlockService
    {

        public Task<bool> HasBlocked(string currentProfileId, string targetProfileId);

        public Task<IList<string>> GetBlockerIds(string profileId);

        public Task<IList<string>> GetBlockeeIds(string profileId);
        public Task<ProfileBlock> GetByBlockerIdAndBlockeeIdAsync(string followerId, string followeeId);
        public Task<ProfileBlock> Block(string followerId, string followeeId);
        public Task<UnBlockResponse> UnBlock(string blockId);
    }
}
