using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.Services.Profile.Application.Exceptions;
using InfinityNetServer.Services.Profile.Application.IServices;
using InfinityNetServer.Services.Profile.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Presentation.Controllers
{
    [Tags("Page profile APIs")]
    [ApiController]
    [Route("pages")]
    public class PageProfileController(
        IAuthenticatedUserService authenticatedUserService,
        CommonFileClient fileClient,
        IStringLocalizer<ProfileSharedResource> localizer,
        ILogger<PageProfileController> logger,
        IMapper mapper,
        IPageProfileService pageProfileService) : BaseApiController(authenticatedUserService)
    {


        [Authorize]
        [EndpointDescription("Retrieve page profile")]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> RetrievePageProfile(string id)
        {
            logger.LogInformation("Retrieve user profile");

            string currentUserId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            PageProfile currentProfile = await pageProfileService.GetById(id);

            var response = mapper.Map<PageProfileResponse>(currentProfile);

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

            return Ok(response);
        }

        [EndpointDescription("Update page profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdatePageProfileRequest request)
        {
            PageProfile pageProfile = mapper.Map<PageProfile>(request);
            try
            {
                pageProfile = await pageProfileService.Update(pageProfile);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Caused by: {Message}", ex.Message);
                throw new ProfileException(ProfileError.UPDATE_PROFILE_FAILED, StatusCodes.Status422UnprocessableEntity);
            }

            return Ok(new
            {
                Message = localizer["Message.ProfileUpdatedSuccess", request.Name],
                User = pageProfile
            });
        }

    }
}
