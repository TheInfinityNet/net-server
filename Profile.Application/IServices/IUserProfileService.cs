using System.Collections.Generic;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;

namespace InfinityNetServer.Services.Profile.Application.IServices
{
    public interface IUserProfileService
    {
        public Task<UserProfile> GetById(string id);

        public Task<UserProfile> GetByAccountId(string id);

        public Task<IList<UserProfile>> GetAllByIds(IList<string> ids);

        public Task<UserProfile> Create(UserProfile userProfile);

        public Task<UserProfile> Update(UserProfile userProfile);

        public Task<CursorPagedResult<UserProfile>> GetFriendSuggestions(string profileId, string cursor, int limit);

        public Task<CursorPagedResult<UserProfile>> GetFriendRequests(string profileId, string cursor, int limit);

        public Task<CursorPagedResult<UserProfile>> GetFriendSentRequests(string profileId, string cursor, int limit);

        public Task<CursorPagedResult<UserProfile>> GetFriends(string profileId, string cursor, int limit);

        public Task<CursorPagedResult<UserProfile>> GetBlockedList(string profileId, string cursor, int limit);

        public Task<CursorPagedResult<UserProfile>> GetFollowingList(string profileId, string cursor, int limit);

        public Task<CursorPagedResult<UserProfile>> GetFollowedList(string profileId, string cursor, int limit);
    }
}
