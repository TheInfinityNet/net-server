using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.Services.Profile.Application.IServices;
using InfinityNetServer.Services.Profile.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Presentation.Controllers
{
    [Tags("User profile APIs")]
    [ApiController]
    [Route("users")]
    public class UserProfileController(
        IAuthenticatedUserService authenticatedUserService,
        CommonFileClient fileClient,
        IStringLocalizer<ProfileSharedResource> localizer,
        ILogger<UserProfileController> logger,
        IMapper mapper,
        CommonRelationshipClient relationshipClient,
        IUserProfileService userProfileService) : BaseApiController(authenticatedUserService)
    {


        [Authorize]
        [EndpointDescription("Retrieve user profile")]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> RetrieveUserProfile(string id)
        {
            logger.LogInformation("Retrieve user profile");

            Guid currentProfileId = GetCurrentProfileId();

            var blockerIds = await relationshipClient.GetAllBlockerIds(currentProfileId.ToString());
            var blockeeIds = await relationshipClient.GetAllBlockeeIds(currentProfileId.ToString());
            if (blockerIds.Concat(blockeeIds).Distinct().Contains(id))
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

            UserProfile currentProfile = await userProfileService.GetById(id);

            var response = mapper.Map<UserProfileResponse>(currentProfile);

            if (currentProfile.AvatarId != null)
            {
                var avatar = await fileClient.GetPhotoMetadata(currentProfile.AvatarId.Value.ToString());
                response.Avatar = avatar;
            }

            if (currentProfile.CoverId != null)
            {
                var cover = await fileClient.GetPhotoMetadata(currentProfile.CoverId.Value.ToString());
                response.Cover = cover;
            }

            //List<string> actions = [];

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

            return Ok(response);
        }

        [EndpointDescription("Update user profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            UserProfile existedProfile = await userProfileService.GetById(GetCurrentProfileId().ToString())
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            UserProfile profile = mapper.Map<UserProfile>(request);
            profile.Id = existedProfile.Id;

            profile = await userProfileService.Update(profile);

            return Ok(new
            {
                Message = localizer["Message.ProfileUpdatedSuccess", request.Username].ToString(),
                User = mapper.Map<UserProfileResponse>(profile)
            });
        }

    }
}
