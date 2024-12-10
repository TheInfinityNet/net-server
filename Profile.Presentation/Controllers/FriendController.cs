using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Relationship;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Profile.Application;
using InfinityNetServer.Services.Profile.Application.DTOs.Responses;
using InfinityNetServer.Services.Profile.Application.IServices;
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
    [Tags("Friends APIs")]
    [ApiController]
    [Route("friends")]
    public class FriendController(
        IAuthenticatedUserService authenticatedUserService,
        IStringLocalizer<ProfileSharedResource> localizer,
        ILogger<FriendController> logger,
        IMapper mapper,
        IProfileService profileService,
        IUserProfileService userProfileService,
        CommonFileClient fileClient,
        CommonRelationshipClient relationshipClient) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Retrieve friend suggestions")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendshipResponse>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("suggestions")]
        public async Task<IActionResult> GetFriendSuggestions([FromQuery] string cursor, [FromQuery] int limit = 100)
        {
            Guid currentProfileId = GetCurrentProfileId();
            var suggestions = await userProfileService.GetFriendSuggestions(currentProfileId.ToString(), cursor, limit);

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

            var resultHasCount = await relationshipClient
                .CountMutualFriends(currentProfileId.ToString(), suggestions.Items.Select(f => f.Id.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendshipResponse> result = [];
            foreach (var item in suggestions.Items)
            {
                var userProfile = mapper.Map<UserProfileResponse>(item);
                if (userProfile.Avatar != null)
                {
                    if (photoMetadataDict.TryGetValue(item.AvatarId, out var avatar))
                    {
                        userProfile.Avatar = avatar;
                    }
                }

                var itemResponse = mapper.Map<FriendshipResponse>(userProfile);
                itemResponse.Status = "NotConnected";
                if (resultHasCountDict.TryGetValue(item.Id.ToString(), out var rs))
                    if (rs.Count > 0) itemResponse.MutualFriendsCount = rs.Count;

                result.Add(itemResponse);
            }

            return Ok(new CursorPagedResult<FriendshipResponse>()
            {
                Items = result,
                NextCursor = suggestions.NextCursor
            });
        }

        [EndpointDescription("search friend suggestions")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendshipResponse>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> SearchFriends([FromQuery] string query, [FromQuery] string cursor, [FromQuery] int limit = 10)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var searchResult = await userProfileService.SearchFriend(query, currentProfileId, cursor, limit);

            var photoMetadataIds = searchResult.Items
                .Where(profile => profile.AvatarId != null)
                    .Select(profile => profile.AvatarId)
                .Distinct();

            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id.Value } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            var resultHasCount = await relationshipClient
                .CountMutualFriends(currentProfileId, searchResult.Items.Select(f => f.Id.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendshipResponse> result = [];
            foreach (var item in searchResult.Items)
            {
                var userProfile = mapper.Map<UserProfileResponse>(item);
                if (userProfile.Avatar != null)
                {
                    if (photoMetadataDict.TryGetValue(item.AvatarId, out var avatar))
                    {
                        userProfile.Avatar = avatar;
                    }
                }

                var itemResponse = mapper.Map<FriendshipResponse>(userProfile);
                itemResponse.Status = "NotConnected";
                if (resultHasCountDict.TryGetValue(item.Id.ToString(), out var rs))
                    if (rs.Count > 0) itemResponse.MutualFriendsCount = rs.Count;

                result.Add(itemResponse);
            }

            return Ok(new CursorPagedResult<FriendshipResponse>()
            {
                Items = result,
                NextCursor = searchResult.NextCursor
            });
        }

        [EndpointDescription("Retrieve blocked list")]
        [ProducesResponseType(typeof(CursorPagedResult<BlockeeResponse>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("blocked-list")]
        public async Task<IActionResult> GetBlookee([FromQuery] string cursor, [FromQuery] int limit = 10)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var blookees = await userProfileService.GetBlockedList(currentProfileId, cursor, limit);

            // Tập hợp toàn bộ các ID cần nạp trước
            var profileIds = blookees.Items.Select(item => item.Id.ToString()).Distinct();
            profileIds.ToList().Add(currentProfileId);

            // Nạp toàn bộ profiles cần thiết
            var profiles = await profileService.GetAllByIds(profileIds.ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

            var photoMetadataIds = profiles
                .Where(profile => profile.AvatarId != null)
                    .Select(profile => profile.AvatarId)
                .Distinct();

            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id.Value } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            var result = blookees.Items
                .Select(up => mapper.Map<BlockeeResponse>(mapper.Map<UserProfileResponse>(up)))
                .ToList();

            CursorPagedResult<BlockeeResponse> response = new()
            {
                Items = result,
                NextCursor = blookees.NextCursor
            };
            return Ok(new
            {
                response
            });
        }

    }
}
