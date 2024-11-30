using System.Collections.Generic;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;

namespace InfinityNetServer.Services.Profile.Application.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetUserProfileById(string id);

        Task<UserProfile> GetUserProfileByAccountId(string id);

        Task<IList<UserProfile>> GetUserProfilesByIds(IList<string> ids);

        Task<CursorPagedResult<UserProfile>> GetFriendSuggestions(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<UserProfile>> GetFriendRequests(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<UserProfile>> GetFriendSentRequests(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<UserProfile>> GetFriends(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<UserProfile>> GetBlockedList(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<UserProfile>> GetFollowingList(string profileId, string cursor, int pageSize);
        Task<CursorPagedResult<UserProfile>> GetFollowedList(string profileId, string cursor, int pageSize);
    }
}
