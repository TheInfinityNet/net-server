using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.Services.Profile.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Application.Services;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Presentation.Controllers
{
    [Tags("User profile APIs")]
    [ApiController]
    [Route("users")]
    public class UserProfileController(
        IAuthenticatedUserService authenticatedUserService,
        IStringLocalizer<ProfileSharedResource> localizer,
        ILogger<UserProfileController> logger,
        IMapper mapper,
        IConfiguration configuration,
        IUserProfileService userProfileService,
        CommonRelationshipClient relationshipClient,
        IMessageBus messageBus) : BaseApiController(authenticatedUserService)
    {


        [EndpointDescription("Update user profile")]
        [HttpPut]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        public IActionResult UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            // TODO: Call the service to update the user profile
            // await _userProfileService.UpdateProfile(request);

            return Ok(new CommonMessageResponse
            (
                localizer["profile_updated_success", request.Username].ToString()
            ));
        }

        [Authorize]
        [EndpointDescription("Retrieve user profile")]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ViewProfileResponse<UserProfileResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RetrieveProfile(string userId)
        {
            logger.LogInformation("Retrieve user profile");

            string currentUserId = authenticatedUserService.GetAuthenticatedProfileId().ToString();

            UserProfile currentProfile = await userProfileService.GetUserProfileById(userId);

            List<string> actions = [];

            if (currentUserId != userId)
            {
                if (await relationshipClient.HasFriendship(currentUserId, userId))
                    actions.Add(ProfileActions.RemoveFriend.ToString());
                else actions.Add(ProfileActions.AddFriend.ToString());

                if (await relationshipClient.HasBlocked(currentUserId, userId))
                    actions.Add(ProfileActions.Unblock.ToString());
                else actions.Add(ProfileActions.Block.ToString());

                if (await relationshipClient.HasFollowed(currentUserId, userId))
                    actions.Add(ProfileActions.Unfollow.ToString());
                else actions.Add(ProfileActions.Follow.ToString());

                if (await relationshipClient.HasFriendRequest(currentUserId, userId))
                    actions.Add(ProfileActions.AcceptOrRejectFriendRequest.ToString());
            }
            else actions.AddRange(
                [ProfileActions.ProfileCoverPhotoUpload.ToString(),
                    ProfileActions.ProfileCoverPhotoDelete.ToString()]);

            return Ok(mapper.Map<UserProfileResponse>(currentProfile));
        }

        [HttpGet("suggestions")]
        [EndpointDescription("Retrieve friend suggestions")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendSuggestionResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetFriendSuggestions([FromQuery] string? nextCursor, [FromQuery] int limit = 10)
        {
            string? currentUserId = authenticatedUserService.GetAuthenticatedProfileId().ToString();

            var suggestions = await userProfileService.GetFriendSuggestions(currentUserId, nextCursor, limit);

            var resultHasCount = await relationshipClient.GetMutualCount(currentUserId, suggestions.Items.Select(f => f.Id.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendSuggestionResponse> result = []; 
            foreach (var item in suggestions.Items)
            {
                var previewProfile = mapper.Map<UserProfileResponse>(item);
                var itemResponse =  mapper.Map<FriendSuggestionResponse>(previewProfile);
                itemResponse.Status = "NotConnected";
                itemResponse.MutualFriendsCount = resultHasCountDict.TryGetValue(item.Id.ToString(), out var a) ? a.Count : 0;
                result.Add(itemResponse);
            }
            CursorPagedResult<FriendSuggestionResponse> a = new (){
                Items = result,
                NextCursor = suggestions.NextCursor
            };
            return Ok(new
            {
                a
            });
        }

        [HttpGet("requests")]
        [EndpointDescription("Retrieve requests")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendSuggestionResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetFriendRequests([FromQuery] string? nextCursor, [FromQuery] int limit = 10)
        {
            string? currentUserId = authenticatedUserService.GetAuthenticatedProfileId().ToString();

            var friendRequests = await userProfileService.GetFriendRequests(currentUserId, nextCursor, limit);

            var resultHasCount = await relationshipClient.GetMutualCount(currentUserId, friendRequests.Items.Select(f => f.Id.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendSuggestionResponse> result = [];
            foreach (var item in friendRequests.Items)
            {
                var previewProfile = mapper.Map<UserProfileResponse>(item);
                var itemResponse = mapper.Map<FriendSuggestionResponse>(previewProfile);
                itemResponse.Status = "RequestReceived";
                itemResponse.MutualFriendsCount = resultHasCountDict.TryGetValue(item.Id.ToString(), out var a) ? a.Count : 0;
                result.Add(itemResponse);
            }
            CursorPagedResult<FriendSuggestionResponse> a = new()
            {
                Items = result,
                NextCursor = friendRequests.NextCursor
            };
            return Ok(new
            {
                Items = result,
                NextCursor = friendRequests.NextCursor
            });
        }

        [HttpGet("sent-requests")]
        [EndpointDescription("Retrieve sent requests ")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendSuggestionResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetFriendSentRequests([FromQuery] string? nextCursor, [FromQuery] int limit = 10)
        {
            string? currentUserId = authenticatedUserService.GetAuthenticatedProfileId().ToString();

            var friendSentRequests = await userProfileService.GetFriendSentRequests(currentUserId, nextCursor, limit);

            var resultHasCount = await relationshipClient.GetMutualCount(currentUserId, friendSentRequests.Items.Select(f => f.Id.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendSuggestionResponse> result = [];
            foreach (var item in friendSentRequests.Items)
            {
                var previewProfile = mapper.Map<UserProfileResponse>(item);
                var itemResponse = mapper.Map<FriendSuggestionResponse>(previewProfile);
                itemResponse.Status = "RequestSent";
                itemResponse.MutualFriendsCount = resultHasCountDict.TryGetValue(item.Id.ToString(), out var a) ? a.Count : 0;
                result.Add(itemResponse);
            }
            CursorPagedResult<FriendSuggestionResponse> a = new()
            {
                Items = result,
                NextCursor = friendSentRequests.NextCursor
            };
            return Ok(new
            {
                Items = result,
                friendSentRequests.NextCursor
            });
        }

        [HttpGet("friends")]
        [EndpointDescription("Retrieve friends")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendSuggestionResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetFriends([FromQuery] string? nextCursor, [FromQuery] int limit = 10)
        {
            string? currentUserId = authenticatedUserService.GetAuthenticatedProfileId().ToString();

            var friends = await userProfileService.GetFriends(currentUserId, nextCursor, limit);

            var resultHasCount = await relationshipClient.GetMutualCount(currentUserId, friends.Items.Select(f => f.Id.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendSuggestionResponse> result = [];
            foreach (var item in friends.Items)
            {
                var previewProfile = mapper.Map<UserProfileResponse>(item);
                var itemResponse = mapper.Map<FriendSuggestionResponse>(previewProfile);
                itemResponse.Status = "Connected";
                itemResponse.MutualFriendsCount = resultHasCountDict.TryGetValue(item.Id.ToString(), out var a) ? a.Count : 0;
                result.Add(itemResponse);
            }
            CursorPagedResult<FriendSuggestionResponse> a = new()
            {
                Items = result,
                NextCursor = friends.NextCursor
            };
            return Ok(new
            {
                Items = result,
                friends.NextCursor
            });
        }
    }
}
