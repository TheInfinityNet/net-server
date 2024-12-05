using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.Services;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.Services.Profile.Infrastructure.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Http;
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
        CommonRelationshipClient relationshipClient
        ) : IUserProfileService
    {
        public async Task<CursorPagedResult<UserProfile>> GetBlockedList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetById(profileId);
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
            var profile = await GetById(profileId);
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
            var profile = await GetById(profileId);
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
            var profile = await GetById(profileId);
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
            var profile = await GetById(profileId);
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
            var profile = await GetById(profileId);
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
            var profile = await GetById(profileId);
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
                        && !blockerIds.Concat(blockeeIds).Contains(userProfile.Id.ToString())
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

        public async Task<UserProfile> GetByAccountId(string id)
            => await _userProfileRepository.GetByAccountIdAsync(Guid.Parse(id));

        public async Task<UserProfile> GetById(string id)
            => await _userProfileRepository.GetByIdAsync(Guid.Parse(id));

        public async Task<IList<UserProfile>> GetAllByIds(IList<string> ids)
            => await _userProfileRepository.GetAllByIdsAsync(ids.Select(Guid.Parse).ToList());

        public async Task<UserProfile> Update(UserProfile userProfile)
        {
            UserProfile existedProfile = await GetById(userProfile.Id.ToString()) 
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            existedProfile.FirstName = userProfile.FirstName;
            existedProfile.LastName = userProfile.LastName;
            existedProfile.Username = userProfile.Username;
            existedProfile.Birthdate = userProfile.Birthdate;
            existedProfile.Gender = userProfile.Gender;
            existedProfile.MobileNumber = userProfile.MobileNumber;

            await _userProfileRepository.UpdateAsync(existedProfile);

            return existedProfile;
        }
    }
}
