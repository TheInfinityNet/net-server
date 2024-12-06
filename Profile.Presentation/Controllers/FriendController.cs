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
        public async Task<IActionResult> GetFriendSuggestions([FromQuery] string nextCursor, [FromQuery] int limit = 10)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var suggestions = await userProfileService.GetFriendSuggestions(currentProfileId, nextCursor, limit);

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
                .CountMutualFriends(currentProfileId, suggestions.Items.Select(f => f.Id.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendshipResponse> result = [];
            foreach (var item in suggestions.Items)
            {
                var userProfile = mapper.Map<UserProfileResponse>(item);
                if (photoMetadataDict.TryGetValue(item.AvatarId, out var avatar))
                {
                    userProfile.Avatar = avatar;
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

        //[EndpointDescription("Retrieve requests")]
        //[ProducesResponseType(typeof(CursorPagedResult<FriendSuggestionResponse>), StatusCodes.Status200OK)]
        //[Authorize]
        //[HttpGet("requests")]
        //public async Task<IActionResult> GetFriendRequests([FromQuery] string nextCursor, [FromQuery] int limit = 10)
        //{
        //    string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString() 
        //        : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

        //    var friendRequests = await userProfileService.GetFriendRequests(currentProfileId, nextCursor, limit);

        //    // Tập hợp toàn bộ các ID cần nạp trước
        //    var profileIds = friendRequests.Items.Select(item => item.Id.ToString()).Distinct();
        //    profileIds.ToList().Add(currentProfileId);

        //    // Nạp toàn bộ profiles cần thiết
        //    var profiles = await profileService.GetAllByIds(profileIds.ToList());
        //    var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

        //    var photoMetadataIds = profiles
        //        .Where(profile => profile.AvatarId != null)
        //            .Select(profile => profile.AvatarId)
        //        .Distinct();

        //    var photoMetadataTasks = photoMetadataIds.Select(async id =>
        //    {
        //        var metadata = await fileClient.GetPhotoMetadata(id.ToString());
        //        return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id.Value } };
        //    });
        //    var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

        //    var resultHasCount = await relationshipClient.GetMutualCount(currentProfileId, friendRequests.Items.Select(f => f.Id.ToString()).ToList());

        //    var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

        //    IList<FriendSuggestionResponse> result = [];
        //    foreach (var item in friendRequests.Items)
        //    {
        //        var previewProfile = mapper.Map<UserProfileResponse>(item);
        //        if (profileDict.TryGetValue(previewProfile.Id, out var profile))
        //        {
        //            var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
        //            previewProfile.Avatar = avatar;
        //        }

        //        var itemResponse = mapper.Map<FriendSuggestionResponse>(previewProfile);
        //        itemResponse.Status = "RequestReceived";
        //        if(resultHasCountDict.TryGetValue(item.Id.ToString(), out var rs))
        //            if (rs.Count > 0) itemResponse.MutualFriendsCount = rs.Count;

        //        result.Add(itemResponse);
        //    }

        //    return Ok(new CursorPagedResult<FriendSuggestionResponse>()
        //    {
        //        Items = result,
        //        NextCursor = friendRequests.NextCursor
        //    });
        //}

        //[EndpointDescription("Retrieve sent requests ")]
        //[ProducesResponseType(typeof(CursorPagedResult<FriendSuggestionResponse>), StatusCodes.Status200OK)]
        //[Authorize]
        //[HttpGet("sent-requests")]
        //public async Task<IActionResult> GetFriendSentRequests([FromQuery] string nextCursor, [FromQuery] int limit = 10)
        //{
        //    string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
        //        : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

        //    var friendSentRequests = await userProfileService.GetFriendSentRequests(currentProfileId, nextCursor, limit);

        //    // Tập hợp toàn bộ các ID cần nạp trước
        //    var profileIds = friendSentRequests.Items.Select(item => item.Id.ToString()).Distinct();
        //    profileIds.ToList().Add(currentProfileId);

        //    // Nạp toàn bộ profiles cần thiết
        //    var profiles = await profileService.GetAllByIds(profileIds.ToList());
        //    var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

        //    var photoMetadataIds = profiles
        //        .Where(profile => profile.AvatarId != null)
        //            .Select(profile => profile.AvatarId)
        //        .Distinct();

        //    var photoMetadataTasks = photoMetadataIds.Select(async id =>
        //    {
        //        var metadata = await fileClient.GetPhotoMetadata(id.ToString());
        //        return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id.Value } };
        //    });
        //    var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

        //    var resultHasCount = await relationshipClient.GetMutualCount(currentProfileId, friendSentRequests.Items.Select(f => f.Id.ToString()).ToList());

        //    var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

        //    IList<FriendSuggestionResponse> result = [];
        //    foreach (var item in friendSentRequests.Items)
        //    {
        //        var previewProfile = mapper.Map<UserProfileResponse>(item);
        //        if (profileDict.TryGetValue(previewProfile.Id, out var profile))
        //        {
        //            var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
        //            previewProfile.Avatar = avatar;
        //        }

        //        var itemResponse = mapper.Map<FriendSuggestionResponse>(previewProfile);
        //        itemResponse.Status = "RequestSent";
        //        if (resultHasCountDict.TryGetValue(item.Id.ToString(), out var rs))
        //            if (rs.Count > 0) itemResponse.MutualFriendsCount = rs.Count;

        //        result.Add(itemResponse);
        //    }

        //    return Ok(new CursorPagedResult<FriendSuggestionResponse>()
        //    {
        //        Items = result,
        //        NextCursor = friendSentRequests.NextCursor
        //    });
        //}

        //[EndpointDescription("Retrieve friends")]
        //[ProducesResponseType(typeof(CursorPagedResult<FriendSuggestionResponse>), StatusCodes.Status200OK)]
        //[Authorize]
        //[HttpGet]
        //public async Task<IActionResult> GetFriends([FromQuery] string nextCursor, [FromQuery] int limit = 10)
        //{
        //    string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
        //        : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

        //    var friends = await userProfileService.GetFriends(currentProfileId, nextCursor, limit);

        //    // Tập hợp toàn bộ các ID cần nạp trước
        //    var profileIds = friends.Items.Select(item => item.Id.ToString()).Distinct();
        //    profileIds.ToList().Add(currentProfileId);

        //    // Nạp toàn bộ profiles cần thiết
        //    var profiles = await profileService.GetAllByIds(profileIds.ToList());
        //    var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<PreviewProfileResponse>);

        //    var photoMetadataIds = profiles
        //        .Where(profile => profile.AvatarId != null)
        //            .Select(profile => profile.AvatarId)
        //        .Distinct();

        //    var photoMetadataTasks = photoMetadataIds.Select(async id =>
        //    {
        //        var metadata = await fileClient.GetPhotoMetadata(id.ToString());
        //        return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id.Value } };
        //    });
        //    var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

        //    var resultHasCount = await relationshipClient.GetMutualCount(currentProfileId, friends.Items.Select(f => f.Id.ToString()).ToList());

        //    var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

        //    IList<FriendSuggestionResponse> result = [];
        //    foreach (var item in friends.Items)
        //    {
        //        var previewProfile = mapper.Map<UserProfileResponse>(item);
        //        if (profileDict.TryGetValue(previewProfile.Id, out var profile))
        //        {
        //            var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
        //            previewProfile.Avatar = avatar;
        //        }

        //        var itemResponse = mapper.Map<FriendSuggestionResponse>(previewProfile);
        //        itemResponse.Status = "Connected";
        //        if (resultHasCountDict.TryGetValue(item.Id.ToString(), out var rs))
        //            if (rs.Count > 0) itemResponse.MutualFriendsCount = rs.Count;

        //        result.Add(itemResponse);
        //    }

        //    return Ok(new CursorPagedResult<FriendSuggestionResponse>()
        //    {
        //        Items = result,
        //        NextCursor = friends.NextCursor
        //    });
        //}

        [EndpointDescription("Retrieve blocked list")]
        [ProducesResponseType(typeof(CursorPagedResult<BlockeeResponse>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("blocked-list")]
        public async Task<IActionResult> GetBlookee([FromQuery] string nextCursor, [FromQuery] int limit = 10)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var blookees = await userProfileService.GetBlockedList(currentProfileId, nextCursor, limit);

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
