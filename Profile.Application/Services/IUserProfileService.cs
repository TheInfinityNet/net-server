using System.Collections.Generic;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;

namespace InfinityNetServer.Services.Profile.Application.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetById(string id);

        Task<UserProfile> GetByAccountId(string id);

        Task<IList<UserProfile>> GetByIds(IList<string> ids);

        Task<UserProfile> Update(UserProfile userProfile);

        Task<CursorPagedResult<UserProfile>> GetFriendSuggestions(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<UserProfile>> GetFriendRequests(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<UserProfile>> GetFriendSentRequests(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<UserProfile>> GetFriends(string profileId, string cursor, int pageSize);
    }
}
