using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.DTOs.Requests;
using InfinityNetServer.Services.Profile.Application.Exceptions;
using InfinityNetServer.Services.Profile.Application.IServices;
using InfinityNetServer.Services.Profile.Domain.Enums;
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
    [Tags("Profile APIs")]
    [ApiController]
    public class ProfileController(
        IAuthenticatedUserService authenticatedUserService,
        IStringLocalizer<ProfileSharedResource> localizer,
        ILogger<ProfileController> logger,
        IMapper mapper,
        IMessageBus messageBus,
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

        [Authorize]
        [EndpointDescription("Upload profile avatar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("avatar")]
        public async Task<IActionResult> UploadAvatar([FromBody] UplodaPhotoRequest avatar)
        {

            Guid profileId = GetCurrentProfileId()
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            Domain.Entities.Profile profile = await profileService.GetById(profileId.ToString())
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            try
            {
                profile.AvatarId = Guid.Parse(avatar.PhotoId);
                profile = await profileService.Update(profile);
                await profileService.ConfirmSave(profile.Id.ToString(), profile.AvatarId.ToString(), false, messageBus);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Caused by: {Message}", ex.Message);
                throw new ProfileException(ProfileError.UPDATE_PROFILE_FAILED, StatusCodes.Status422UnprocessableEntity);
            }

            return Ok(new
            {
                Message = localizer["Message.ProfileUpdatedSuccess", profile.Id].ToString(),
                Profile = mapper.Map<BaseProfileResponse>(profile)
            });
        }

        [Authorize]
        [EndpointDescription("Upload profile cover")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("cover")]
        public async Task<IActionResult> UploadCover([FromBody] UplodaPhotoRequest request)
        {

            Guid profileId = GetCurrentProfileId()
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            Domain.Entities.Profile profile = await profileService.GetById(profileId.ToString())
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            try
            {
                profile.CoverId = Guid.Parse(request.PhotoId);
                profile = await profileService.Update(profile);
                await profileService.ConfirmSave(profile.Id.ToString(), profile.CoverId.ToString(), false, messageBus);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Caused by: {Message}", ex.Message);
                throw new ProfileException(ProfileError.UPDATE_PROFILE_FAILED, StatusCodes.Status422UnprocessableEntity);
            }

            return Ok(new
            {
                Message = localizer["Message.ProfileUpdatedSuccess", profile.Id].ToString(),
                Profile = mapper.Map<BaseProfileResponse>(profile)
            });
        }

    }
}
