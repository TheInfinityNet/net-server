using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.IServices;
using InfinityNetServer.Services.Profile.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Profile.Presentation.Controllers
{
    [Tags("Profile actions APIs")]
    [ApiController]
    public class ProfileActionController(
        IAuthenticatedUserService authenticatedUserService,
        IStringLocalizer<ProfileSharedResource> localizer,
        ILogger<ProfileActionController> logger,
        IMapper mapper,
        IProfileService profileService) : BaseApiController(authenticatedUserService)
    {


        [Authorize]
        [EndpointDescription("Retrieve page profile")]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{id}/actions")]
        public IActionResult GetProfileActions(string id)
        {
            logger.LogInformation("Retrieve user profile");

            Guid currentUserId = GetCurrentProfileId != null ? GetCurrentProfileId().Value
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            Guid.TryParse(id, out Guid profileId);

            bool isMe = currentUserId == profileId;

            IDictionary<string, bool> actions = new Dictionary<string, bool>
            {
                { ProfileActions.ProfileAvatarUpload.ToString(), isMe },
                { ProfileActions.ProfileCoverPhotoUpload.ToString(), isMe },
                { ProfileActions.ProfileAvatarDelete.ToString(), isMe },
                { ProfileActions.ProfileCoverPhotoDelete.ToString(), isMe },
                { ProfileActions.ProfileDetailsUpdate.ToString(), isMe },
                { ProfileActions.ProfileCreate.ToString(), isMe },
                { ProfileActions.ProfileDelete.ToString(), isMe },
                { ProfileActions.ProfileStatusLock.ToString(), !isMe },
                { ProfileActions.ProfileActivityLog.ToString(), isMe },
                { ProfileActions.ProfilePostCreate.ToString(), isMe },
                { ProfileActions.ProfilePostSearch.ToString(), isMe },
                { ProfileActions.ProfileReport.ToString(), !isMe }
            };


            return Ok(actions);
        }

    }
}
