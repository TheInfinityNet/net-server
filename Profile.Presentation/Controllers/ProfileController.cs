﻿using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
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
using System.Linq;
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
        CommonFileClient fileClient,
        IProfileService profileService) : BaseApiController(authenticatedUserService)
    {


        [Authorize]
        [EndpointDescription("Get profile actions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("{id}/actions")]
        public IActionResult GetProfileActions(string id)
        {
            logger.LogInformation("Retrieve user profile");

            bool isMe = IsOwner(id);

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
        [HttpPut("avatar")]
        public async Task<IActionResult> UploadAvatar([FromBody] UplodaPhotoRequest avatar)
        {
            Domain.Entities.Profile profile = await profileService.GetById(GetCurrentProfileId().ToString())
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            try
            {
                profile.AvatarId = Guid.Parse(avatar.PhotoId);
                profile = await profileService.Update(profile);
                await profileService.ConfirmSave(profile.Id.ToString(), profile.AvatarId.ToString(), true);
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
        [HttpPut("cover")]
        public async Task<IActionResult> UploadCover([FromBody] UplodaPhotoRequest request)
        {
            Domain.Entities.Profile profile = await profileService.GetById(GetCurrentProfileId().ToString())
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            try
            {
                profile.CoverId = Guid.Parse(request.PhotoId);
                profile = await profileService.Update(profile);
                await profileService.ConfirmSave(profile.Id.ToString(), profile.CoverId.ToString(), false);
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

        [EndpointDescription("Seacrch profiles")]
        [ProducesResponseType(typeof(CursorPagedResult<BaseProfileResponse>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchFriends([FromQuery] string keywords, [FromQuery] string nextCursor, [FromQuery] int limit = 10)
        {
            var suggestions = await profileService.Search(GetCurrentProfileId().ToString(), keywords, nextCursor, limit);

            var photoMetadataIds = suggestions.Items
                .Where(profile => profile.AvatarId != null)
                    .Select(profile => profile.AvatarId)
                .Distinct();

            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id.Value } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            IList<BaseProfileResponse> result = [];
            foreach (var item in suggestions.Items)
            {
                var profile = mapper.Map<BaseProfileResponse>(item);
                if (profile.Avatar != null)
                {
                    if (photoMetadataDict.TryGetValue(item.AvatarId, out var avatar))
                    {
                        profile.Avatar = avatar;
                    }
                }

                result.Add(profile);
            }

            return Ok(new CursorPagedResult<BaseProfileResponse>()
            {
                Items = result,
                NextCursor = suggestions.NextCursor
            });
        }

    }
}
