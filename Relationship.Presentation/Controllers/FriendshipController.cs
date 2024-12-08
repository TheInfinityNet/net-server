using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Relationship;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Relationship.Application;
using InfinityNetServer.Services.Relationship.Application.DTOs.Requests;
using InfinityNetServer.Services.Relationship.Application.GrpcClients;
using InfinityNetServer.Services.Relationship.Application.IServices;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Presentation.Controllers
{

    [Tags("Friends APIs")]
    [ApiController]
    [Route("friends")]
    public class FriendshipController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<FriendshipController> logger,
        IStringLocalizer<RelationshipSharedResource> Localizer,
        IMessageBus messageBus,
        IMapper mapper,
        IFriendshipService friendshipService,
        IProfileBlockService profileBlockService,
        IProfileFollowService profileFollowService,
        CommonProfileClient commonProfileClient,
        CommonFileClient fileClient,
        ProfileClient profileClient) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Count friendships of a profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("count/{profileId}")]
        public async Task<IActionResult> CountFriendships(string profileId)
        {
            int count = await friendshipService.CountFriendships(profileId);
            return Ok(count);
        }

        [EndpointDescription("Get friends of a profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("preview/{profileId}")]
        public async Task<IActionResult> GetFriends(string profileId)
        {
            IList<string> friendIds = await friendshipService.GetPreviewFriendIds(profileId);
            IList<UserProfileResponse> friends = await profileClient.GetPreviewFriendsOfProfile(friendIds);
            int totalFriends = await friendshipService.CountFriendships(profileId);
            return Ok(new
            {
                friends,
                totalFriends
            });
        }

        [EndpointDescription("Retrieve requests")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendshipResponse>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("requests")]
        public async Task<IActionResult> GetFriendRequests([FromQuery] string nextCursor, [FromQuery] int limit = 10)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var friendRequests = await friendshipService.GetFriendRequests(currentProfileId, nextCursor, limit);

            // Tập hợp toàn bộ các ID cần nạp trước
            var profileIds = friendRequests.Items.Select(item => item.SenderId.ToString()).Distinct();

            // Nạp toàn bộ profiles cần thiết
            var profiles = await commonProfileClient.GetProfiles(profileIds.ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<BaseProfileResponse>);

            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Distinct();

            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            var resultHasCount = await friendshipService.CountMutualFriends(currentProfileId, friendRequests.Items.Select(f => f.SenderId.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendshipResponse> result = [];
            foreach (var item in friendRequests.Items)
            {
                if (profileDict.TryGetValue(item.SenderId, out var profile))
                {
                    if (profile.Avatar != null)
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
                        profile.Avatar = avatar;
                    }
                }

                var itemResponse = mapper.Map<FriendshipResponse>(profile);
                itemResponse.Status = "RequestReceived";
                itemResponse.CreatedAt = item.CreatedAt;
                if (resultHasCountDict.TryGetValue(item.SenderId.ToString(), out var rs))
                    if (rs.Count > 0) itemResponse.MutualFriendsCount = rs.Count;

                result.Add(itemResponse);
            }

            return Ok(new CursorPagedResult<FriendshipResponse>()
            {
                Items = result,
                NextCursor = friendRequests.NextCursor
            });
        }

        [EndpointDescription("Retrieve sent requests ")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendshipResponse>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet("sent-requests")]
        public async Task<IActionResult> GetFriendSentRequests([FromQuery] string nextCursor, [FromQuery] int limit = 10)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var friendSentRequests = await friendshipService.GetFriendSentRequests(currentProfileId, nextCursor, limit);

            // Tập hợp toàn bộ các ID cần nạp trước
            var profileIds = friendSentRequests.Items.Select(item => item.ReceiverId.ToString()).Distinct();
            profileIds.ToList().Add(currentProfileId);

            // Nạp toàn bộ profiles cần thiết
            var profiles = await commonProfileClient.GetProfiles(profileIds.ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<BaseProfileResponse>);

            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Distinct();

            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            var resultHasCount = await friendshipService.CountMutualFriends(currentProfileId, friendSentRequests.Items.Select(f => f.ReceiverId.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendshipResponse> result = [];
            foreach (var item in friendSentRequests.Items)
            {
                if (profileDict.TryGetValue(item.ReceiverId, out var profile))
                {
                    if (profile.Avatar != null)
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
                        profile.Avatar = avatar;
                    }
                }

                var itemResponse = mapper.Map<FriendshipResponse>(profile);
                itemResponse.Status = "RequestSent";
                if (resultHasCountDict.TryGetValue(item.ReceiverId.ToString(), out var rs))
                    if (rs.Count > 0) itemResponse.MutualFriendsCount = rs.Count;

                result.Add(itemResponse);
            }

            return Ok(new CursorPagedResult<FriendshipResponse>()
            {
                Items = result,
                NextCursor = friendSentRequests.NextCursor
            });
        }

        [EndpointDescription("Retrieve friends")]
        [ProducesResponseType(typeof(CursorPagedResult<FriendshipResponse>), StatusCodes.Status200OK)]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetFriends([FromQuery] string nextCursor, [FromQuery] int limit = 10)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var friends = await friendshipService.GetFriends(currentProfileId, nextCursor, limit);

            // Tập hợp toàn bộ các ID cần nạp trước
            var profileIds = friends.Items.Select(item => item.SenderId.ToString()).Distinct();
            profileIds.ToList().Add(currentProfileId);

            // Nạp toàn bộ profiles cần thiết
            var profiles = await commonProfileClient.GetProfiles(profileIds.ToList());
            var profileDict = profiles.ToDictionary(p => p.Id, mapper.Map<BaseProfileResponse>);

            var photoMetadataIds = profiles
                .Where(profile => profile.Avatar != null)
                    .Select(profile => profile.Avatar.Id)
                .Distinct();

            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                var metadata = await fileClient.GetPhotoMetadata(id.ToString());
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id } };
            });
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            var resultHasCount = await friendshipService.CountMutualFriends(currentProfileId, friends.Items.Select(f => f.SenderId.ToString()).ToList());

            var resultHasCountDict = resultHasCount.ToDictionary(p => p.ProfileId);

            IList<FriendshipResponse> result = [];
            foreach (var item in friends.Items)
            {
                if (profileDict.TryGetValue(item.SenderId, out var profile))
                {
                    if (profile.Avatar != null)
                    {
                        var avatar = photoMetadataDict.GetValueOrDefault(profile.Avatar.Id);
                        profile.Avatar = avatar;
                    }
                }

                var itemResponse = mapper.Map<FriendshipResponse>(profile);
                itemResponse.Status = "Connected";
                if (resultHasCountDict.TryGetValue(item.SenderId.ToString(), out var rs))
                    if (rs.Count > 0) itemResponse.MutualFriendsCount = rs.Count;

                result.Add(itemResponse);
            }

            return Ok(new CursorPagedResult<FriendshipResponse>()
            {
                Items = result,
                NextCursor = friends.NextCursor
            });
        }

        [EndpointDescription("Send a friend request")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize]
        [HttpPost("requests")]
        public async Task<IActionResult> SendRequest([FromBody] RelationshipRequest request)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var requestResponse = await friendshipService.SendRequest(currentProfileId, request.UserId);

            try
            {
                await profileFollowService.Follow(currentProfileId, request.UserId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return Ok(requestResponse);
        }

        [EndpointDescription("Cancel a friend request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpDelete("requests/{profileId}")]
        public async Task<IActionResult> CancelRequest(string profileId)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var friendship = await friendshipService.GetByStatus(FriendshipStatus.Pending, currentProfileId.ToString(), profileId) 
                ?? throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);

            if (friendship.SenderId != Guid.Parse(currentProfileId))
            {
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);
            }

            var result = await friendshipService.CancelRequest(friendship.Id);
            return Ok(
                result
            );
        }

        [EndpointDescription("Accept a friend request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut("requests/accept")]
        public async Task<IActionResult> AcceptRequest([FromBody] RelationshipRequest request)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var friendship = await friendshipService.GetByStatus(FriendshipStatus.Pending, request.UserId, currentProfileId.ToString()) 
                ?? throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);

            if (friendship.ReceiverId != Guid.Parse(currentProfileId))
            {
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);
            }

            var requestResponse = await friendshipService.AcceptRequest(friendship);

            try
            {
                await profileFollowService.Follow(currentProfileId, request.UserId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            return Ok(requestResponse);
        }

        [EndpointDescription("Decline a friend request")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPut("requests/decline")]
        public async Task<IActionResult> DeclineRequest([FromBody] RelationshipRequest request)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var friendship = await friendshipService.GetByStatus(FriendshipStatus.Pending, request.UserId, currentProfileId.ToString())
                ?? throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);

            if (friendship.ReceiverId != Guid.Parse(currentProfileId))
            {
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);
            }
            var requestResponse = await friendshipService.DeclineRequest(friendship.Id);
            return Ok(requestResponse);
        }

        [EndpointDescription("Unfriend a profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> UnFriend(string userId)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var friendship = await friendshipService.GetByStatus(FriendshipStatus.Connected, userId, currentProfileId.ToString())
                ?? throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);

            if (friendship.ReceiverId != Guid.Parse(currentProfileId) && friendship.SenderId != Guid.Parse(currentProfileId))
            {
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);
            }
            var result = await friendshipService.Unfriend(friendship.Id);
            return Ok(
                result
            );
        }

        [EndpointDescription("Follow a profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] string followeeId)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var followResponse = await profileFollowService.Follow(currentProfileId.ToString(), followeeId);
            return Ok(followResponse);
        }

        [EndpointDescription("Unfollow a profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpDelete("follow/{profileId}")]
        public async Task<IActionResult> UnFollow(string profileId)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var followee = await profileFollowService.GetByFollowerIdAndFolloweeIdAsync(currentProfileId.ToString(), profileId) 
                ?? throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);

            if (followee.FollowerId != Guid.Parse(currentProfileId) && followee.FolloweeId != Guid.Parse(currentProfileId))
            {
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);
            }

            var result = await profileFollowService.UnFollow(followee.Id.ToString());
            return Ok(
                result
            );
        }

        [EndpointDescription("Block a profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpPost("block")]
        public async Task<IActionResult> Block([FromBody] string followeeId)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var followee = await profileFollowService.GetByFollowerIdAndFolloweeIdAsync(currentProfileId.ToString(), followeeId);
            if (followee != null)
            {
                await profileFollowService.UnFollow(followee.Id.ToString());
            }

            var followee1 = await profileFollowService.GetByFollowerIdAndFolloweeIdAsync(followeeId, currentProfileId.ToString());
            if (followee1 != null)
            {
                await profileFollowService.UnFollow(followee1.Id.ToString());
            }

            var friendShip = await friendshipService.HasFriendship(currentProfileId.ToString(), followeeId);
            if (friendShip != null)
            {
                await friendshipService.Unfriend(friendShip.Id);
            }

            var blockResponse = await profileBlockService.Block(currentProfileId.ToString(), followeeId);
            return Ok(blockResponse);
        }

        [EndpointDescription("Unblock a profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        [HttpDelete("block/{profileId}")]
        public async Task<IActionResult> UnBlock(string profileId)
        {
            string currentProfileId = GetCurrentProfileId != null ? GetCurrentProfileId().ToString()
                : throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            var blockee = await profileBlockService.GetByBlockerIdAndBlockeeIdAsync(currentProfileId.ToString(), profileId) 
                ?? throw new BaseException(BaseError.RELATIONSHIP_NOT_FOUND, StatusCodes.Status404NotFound);

            var result = await profileBlockService.UnBlock(blockee.Id.ToString());
            return Ok(
                result
            );
        }
    }
}
