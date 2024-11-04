using System.Collections.Generic;
using InfinityNetServer.Services.Profile.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.BuildingBlocks.Application.Bus;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.Services.Profile.Application.Services;
using Microsoft.AspNetCore.Authorization;
using InfinityNetServer.Services.Profile.Domain.Entities;
using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Profile.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;

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

        //[Authorize]
        [EndpointDescription("Retrieve user profile")]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ViewProfileResponse<UserProfileResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RetrieveProfile(string userId)
        {
            logger.LogInformation("Retrieve user profile");

            //string currentUserId = authenticatedUserService.GetAuthenticatedUserId().ToString();

            UserProfile currentProfile = await userProfileService.GetUserProfileById(userId);

            List<string> actions = [];

            //if (currentUserId != userId)
            //{
            //    if (await relationshipClient.HasFriendship(currentUserId, userId))
            //        actions.Add(ProfileActions.RemoveFriend.ToString());
            //    else actions.Add(ProfileActions.AddFriend.ToString());

            //    if (await relationshipClient.HasBlocked(currentUserId, userId))
            //        actions.Add(ProfileActions.Unblock.ToString());
            //    else actions.Add(ProfileActions.Block.ToString());

            //    if (await relationshipClient.HasFollowed(currentUserId, userId))
            //        actions.Add(ProfileActions.Unfollow.ToString());
            //    else actions.Add(ProfileActions.Follow.ToString());

            //    if (await relationshipClient.HasMuted(currentUserId, userId))
            //        actions.Add(ProfileActions.UnMute.ToString());
            //    else actions.Add(ProfileActions.Mute.ToString());

            //    if (await relationshipClient.HasFriendRequest(currentUserId, userId))
            //        actions.Add(ProfileActions.AcceptOrRejectFriendRequest.ToString());
            //}
            //else actions.AddRange(
            //    [ProfileActions.ProfileCoverPhotoUpload.ToString(), 
            //        ProfileActions.ProfileCoverPhotoDelete.ToString()]);

            return Ok(mapper.Map<UserProfileResponse>(currentProfile));
        }

    }
}
