using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.Services.Profile.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Application.Services;
using InfinityNetServer.Services.Profile.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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

        [Authorize]
        [EndpointDescription("Update user profile")]
        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(CommonMessageResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            await userProfileService.UpdateUserProfile(request);

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
            logger.LogInformation("Retrieve user profile for userId: {userId}", userId);

            //string currentUserId = authenticatedUserService.GetAuthenticatedUserId().ToString();

            UserProfile currentProfile = await userProfileService.GetUserProfileById(userId);

            List<string> actions = new List<string>();

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

            return Ok(mapper.Map<UserProfileResponse>(currentProfile));
        }

    }
}
