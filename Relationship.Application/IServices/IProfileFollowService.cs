using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Application.IServices
{
    public interface IProfileFollowService
    {

        public Task<bool> HasFollowed(string currentProfileId, string targetProfileId);

        public Task<IList<string>> GetAllFollowerIds(string currentProfileId);

        public Task<IList<string>> GetAllFolloweeIds(string currentProfileId);

        public Task<ProfileFollow> GetByFollowerIdAndFolloweeIdAsync(string followerId, string followeeId);

        public Task<CursorPagedResult<ProfileFollow>> GetFollowers(string profileId, string cursor, int limit);

        public Task<CursorPagedResult<ProfileFollow>> GetFollowees(string profileId, string cursor, int limit);

        public Task<ProfileFollow> Follow(string followerId, string followeeId);

        public Task<UnFollowResponse> UnFollow(string followId);

    }
}
