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
        public async Task<CursorPagedResult<UserProfile>> GetBlockedList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetUserProfileById(profileId);
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(profileId.ToString());

            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        blockeeIds.Contains(userProfile.Id.ToString()),
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

        public async Task<CursorPagedResult<UserProfile>> GetFollowedList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetUserProfileById(profileId);
            IList<string> followerIds = await relationshipClient.GetFollowerIds(profileId);

            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        followerIds.Contains(userProfile.Id.ToString()),
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
        public async Task<CursorPagedResult<UserProfile>> GetFollowingList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetUserProfileById(profileId);
            IList<string> followeeIds = await relationshipClient.GetFolloweeIds(profileId);

            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        followeeIds.Contains(userProfile.Id.ToString()),
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
        public async Task<CursorPagedResult<UserProfile>> GetFriendRequests(string profileId, string cursor, int pageSize)
        {
            var profile = await GetUserProfileById(profileId);
            IList<string> pendingRequests = await relationshipClient.GetRequestsProfiles(profileId);
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(profileId.ToString());

            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        pendingRequests.Contains(userProfile.Id.ToString())
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

        public async Task<CursorPagedResult<UserProfile>> GetFriends(string profileId, string cursor, int pageSize)
        {
            var profile = await GetUserProfileById(profileId);
            IList<string> pendingRequests = await relationshipClient.GetFriendIds(profileId);
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(profileId.ToString());
            
            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        pendingRequests.Contains(userProfile.Id.ToString())
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

        public async Task<CursorPagedResult<UserProfile>> GetFriendSentRequests(string profileId, string cursor, int pageSize)
        {
            var profile = await GetUserProfileById(profileId);
            IList<string> pendingRequests = await relationshipClient.GetSentRequestProfiles(profileId);
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(profileId.ToString());

            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        pendingRequests.Contains(userProfile.Id.ToString())
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

        public async Task<UserProfile> GetUserProfileByAccountId(string id)
        {
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
