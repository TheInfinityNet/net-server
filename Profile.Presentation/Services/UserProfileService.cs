using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.Services;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.Services.Profile.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Presentation.Services
{
    public class UserProfileService(
        IUserProfileRepository _userProfileRepository,
        ILogger<UserProfileService> _logger,
        IStringLocalizer<ProfileSharedResource> _localizer,
        CommonRelationshipClient relationshipClient
        ) : IUserProfileService
    {

        public async Task<CursorPagedResult<UserProfile>> GetFriendSuggestions(string profileId, string cursor, int pageSize)
        {
            var profile = await GetUserProfileById(profileId);
            IList<string> followeeIds = await relationshipClient.GetFolloweeIds(profileId);
            IList<string> friendIds = await relationshipClient.GetFriendIds(profileId);
            IList<string> pendingRequests = await relationshipClient.GetPendingRequestProfiles(profileId);
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(profileId.ToString());

            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        !friendIds.Contains(userProfile.Id.ToString())
                        && !pendingRequests.Contains(userProfile.Id.ToString())
                        && !blockerIds.Concat(blockeeIds).Contains(userProfile.Id.ToString()),

                OrderFields = [
                        new OrderField<UserProfile>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                PageSize = pageSize
            };
            return await _userProfileRepository.GetPagedAsync(specification);
        }

        // await async là kiến thức về bất đồng bộ có j ông search youtube xem thêm nha
        public async Task<UserProfile> GetUserProfileByAccountId(string id)
        {
            // chỗ này implement cái đã định nghĩa trong interface
            // truyền id vào là string nên phải parse ra Guid
            return await _userProfileRepository.GetUserProfileByAccountIdAsync(Guid.Parse(id));
        }

        public async Task<UserProfile> GetUserProfileById(string id)
        {
            return await _userProfileRepository.GetByIdAsync(Guid.Parse(id));
        }

        public async Task<IList<UserProfile>> GetUserProfilesByIds(IList<string> ids)
        {
            return await _userProfileRepository.GetUserProfilesByIdsAsync(ids.Select(Guid.Parse).ToList());
        }
    }
}
