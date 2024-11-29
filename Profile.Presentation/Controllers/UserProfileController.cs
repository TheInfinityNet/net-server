using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.Services.Profile.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Application.Exceptions;
using InfinityNetServer.Services.Profile.Application.Services;
using InfinityNetServer.Services.Profile.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        IUserProfileService userProfileService) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Retrieve user profile")]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> RetrieveUserProfile(string id)
        {
            logger.LogInformation("Retrieve user profile");

            string currentUserId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            UserProfile currentProfile = await userProfileService.GetById(id);

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

            //    if (await relationshipClient.HasFriendRequest(currentUserId, userId))
            //        actions.Add(ProfileActions.AcceptOrRejectFriendRequest.ToString());
            //}
            //else actions.AddRange(
            //    [ProfileActions.ProfileCoverPhotoUpload.ToString(),
            //        ProfileActions.ProfileCoverPhotoDelete.ToString()]);

            return Ok(mapper.Map<UserProfileResponse>(currentProfile));
        }

        [EndpointDescription("Update user profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            UserProfile userProfile = mapper.Map<UserProfile>(request);
            try
            {
                userProfile = await userProfileService.Update(userProfile);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Caused by: {Message}", ex.Message);
                throw new ProfileException(ProfileError.UPDATE_PROFILE_FAILED, StatusCodes.Status422UnprocessableEntity);
            }

            return Ok(new
            {
                Message = localizer["Message.ProfileUpdatedSuccess", request.Username],
                User = userProfile
            });
        }

    }
}
