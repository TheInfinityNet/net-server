using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Profile.Application.IServices;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.Services
{
    public class UserProfileService(
        IUserProfileRepository userProfileRepository,
        ILogger<UserProfileService> logger,
        CommonRelationshipClient relationshipClient) : IUserProfileService
    {
        public async Task<CursorPagedResult<UserProfile>> GetBlockedList(string profileId, string cursor, int limit)
        {
            var profile = await GetById(profileId);
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId.ToString());

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
                Limit = limit
            };
            return await userProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<UserProfile>> GetFollowedList(string profileId, string cursor, int limit)
        {
            var profile = await GetById(profileId);
            IList<string> followerIds = await relationshipClient.GetAllFollowerIds(profileId);

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
                Limit = limit
            };
            return await userProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<UserProfile>> GetFollowingList(string profileId, string cursor, int limit)
        {
            var profile = await GetById(profileId);
            IList<string> followeeIds = await relationshipClient.GetAllFolloweeIds(profileId);

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
                Limit = limit
            };
            return await userProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<UserProfile>> GetFriendRequests(string profileId, string cursor, int limit)
        {
            var profile = await GetById(profileId);
            IList<string> pendingRequests = await relationshipClient.GetAllRequestIds(profileId);
            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId.ToString());

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
                Limit = limit
            };
            return await userProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<UserProfile>> GetFriends(string profileId, string cursor, int limit)
        {
            var profile = await GetById(profileId);
            IList<string> pendingRequests = await relationshipClient.GetAllFriendIds(profileId);
            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId.ToString());

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
                Limit = limit
            };
            return await userProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<UserProfile>> GetFriendSentRequests(string profileId, string cursor, int limit)
        {
            var profile = await GetById(profileId);
            IList<string> pendingRequests = await relationshipClient.GetAllSentRequestIds(profileId);
            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId.ToString());

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
                Limit = limit
            };
            return await userProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<UserProfile>> GetFriendSuggestions(string profileId, string cursor, int limit)
        {
            //IList<string> followeeIds = await relationshipClient.GetAllFolloweeIds(profileId);
            //IList<string> friendIds = await relationshipClient.GetAllFriendIds(profileId);
            //var friendsOfMutualFriends = await relationshipClient.GetFriendsOfMutualFriends(profileId);
            IList<string> pendingRequests = await relationshipClient.GetAllPendingRequestIds(profileId);
            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId.ToString());
            //var mutualFriendIds = friendsOfMutualFriends
            //.Select(item => item.ProfileId)
            //.ToList();
            var suggestionIds = await userProfileRepository.GetTopProfileAsync(50);
            var suggestionIdStrings = suggestionIds.Select(id => id.ToString()).ToList();
            //var mergedIds = mutualFriendIds
            //    .Union(suggestionIdStrings) 
            //    .ToList();

            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        //suggestionIds.Contains(userProfile.Id)
                         !userProfile.Id.Equals(Guid.Parse(profileId))
                        //&& !friendIds.Contains(userProfile.Id.ToString())
                        & !pendingRequests.Contains(userProfile.Id.ToString())
                        & !blockerIds.Concat(blockeeIds).Contains(userProfile.Id.ToString())
                        & !blockerIds.Concat(blockeeIds).Contains(userProfile.Id.ToString()),
                Cursor = cursor,
                Limit = limit
            };
            var ok =  await userProfileRepository.GetPagedAsync(specification);
            return ok;
        }

        public async Task<CursorPagedResult<UserProfile>> SearchFriend(string keywords, string profileId, string cursor, int limit)
        {

            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId.ToString());

            var specification = new SpecificationWithCursor<UserProfile>
            {
                Criteria = userProfile =>
                        (string.IsNullOrEmpty(keywords) ||
                         userProfile.FirstName.Contains(keywords, StringComparison.CurrentCultureIgnoreCase) ||
                         userProfile.LastName.Contains(keywords, StringComparison.CurrentCultureIgnoreCase) ||
                         userProfile.Username.Contains(keywords, StringComparison.CurrentCultureIgnoreCase))
                        &!blockerIds.Concat(blockeeIds).Contains(userProfile.Id.ToString()),
                Cursor = cursor,
                Limit = limit
            };
            return await userProfileRepository.GetPagedAsync(specification);
        }

        public async Task<UserProfile> GetByAccountId(string id)
            => await userProfileRepository.GetByAccountIdAsync(Guid.Parse(id));

        public async Task<UserProfile> GetById(string id)
            => await userProfileRepository.GetByIdAsync(Guid.Parse(id));

        public async Task<IList<UserProfile>> GetAllByIds(IList<string> ids)
            => await userProfileRepository.GetAllByIdsAsync(ids.Select(Guid.Parse).ToList());

        public async Task<UserProfile> Update(UserProfile userProfile)
        {
            UserProfile existedProfile = await GetById(userProfile.Id.ToString())
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            existedProfile.AvatarId = userProfile.AvatarId;
            existedProfile.CoverId = userProfile.CoverId;
            existedProfile.Location = userProfile.Location;
            existedProfile.MobileNumber = userProfile.MobileNumber;
            existedProfile.Status = userProfile.Status;

            existedProfile.FirstName = userProfile.FirstName;
            existedProfile.LastName = userProfile.LastName;
            existedProfile.Username = userProfile.Username;
            existedProfile.Birthdate = userProfile.Birthdate;
            existedProfile.Gender = userProfile.Gender;
            existedProfile.Bio = userProfile.Bio;

            await userProfileRepository.UpdateAsync(existedProfile);

            return existedProfile;
        }

        public async Task<UserProfile> Create(UserProfile userProfile)
        {
            logger.LogInformation("Create user profile");
            return await userProfileRepository.CreateAsync(userProfile);
        }
    }
}
