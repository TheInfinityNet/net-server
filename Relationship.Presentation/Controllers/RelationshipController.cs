using System.Collections.Generic;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Relationship.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using InfinityNetServer.Services.Relationship.Application.GrpcClients;
using InfinityNetServer.Services.Relationship.Application.Services;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using System;
using AutoMapper;
using InfinityNetServer.Services.Relationship.Application.DTOs.Responses;
using MassTransit.Initializers;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Relationship.Presentation.Controllers
{
    [ApiController]
    [Route("friends")]
    public class RelationshipController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<RelationshipController> logger,
        IStringLocalizer<RelationshipSharedResource> Localizer,
        IMessageBus messageBus,
        IMapper mapper,
        IFriendshipService friendshipService,
        IProfileBlockService profileBlockService,
        IProfileFollowService profileFollowService,
        ProfileClient profileClient) : BaseApiController(authenticatedUserService)
    {
        [HttpGet("count/{profileId}")]
        public async Task<IActionResult> CountFriendships(string profileId)
        {
            int count = await friendshipService.CountFriendships(profileId);
            return Ok(count);
        }

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
        
        [HttpPost("requests")]
        [Authorize]
        public async Task<IActionResult> SendRequest([FromBody] string request)
        {
            Guid? currentUserId = GetCurrentProfileId();
            var requestResponse = await friendshipService.SendRequest(currentUserId.ToString(), request);
            return Ok(requestResponse);
        }

        [HttpDelete("requests/{profileId}")]
        [Authorize]
        public async Task<IActionResult> CancelRequest(string profileId)
        {
            Guid? currentUserId = GetCurrentProfileId();
            Domain.Entities.Friendship friendship = await friendshipService.GetByStatus(FriendshipStatus.Pending, currentUserId.ToString(), profileId);
            if (friendship == null)
            {
                return NotFound();
            }
            var result = await friendshipService.CancelRequest(friendship.Id);
            return Ok(
                result
            );
        }

        [HttpDelete("{profileId}")]
        [Authorize]
        public async Task<IActionResult> UnFriend(string profileId)
        {
            Guid? currentUserId = GetCurrentProfileId();
            Domain.Entities.Friendship friendship = await friendshipService.GetByStatus(FriendshipStatus.Pending, currentUserId.ToString(), profileId);
            if (friendship == null)
            {
                return NotFound();
            }
            var result = await friendshipService.RejectRequest(friendship.Id);
            return Ok(
                result
            );
        }

        [HttpPost("requests/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptRequest([FromBody] string request)
        {
            Guid? currentUserId = GetCurrentProfileId();
            var requestResponse = await friendshipService.AcceptRequest(currentUserId.ToString(), request);
            return Ok(requestResponse);
        }

        [HttpPost("requests/decline")]
        [Authorize]
        public async Task<IActionResult> RejectRequest([FromBody] string request)
        {
            Guid? currentUserId = GetCurrentProfileId();
            var requestResponse = await friendshipService.RejectRequest(Guid.Parse(request));
            return Ok(requestResponse);
        }

        [HttpPost("follow")]
        [Authorize]
        public async Task<IActionResult> Follow([FromBody] string followeeId)
        {
            Guid? currentUserId = GetCurrentProfileId();
            var followResponse = await profileFollowService.Follow(currentUserId.ToString(), followeeId);
            return Ok(followResponse);
        }

        [HttpDelete("follow/{profileId}")]
        [Authorize]
        public async Task<IActionResult> UnFollow(string profileId)
        {
            Guid? currentUserId = GetCurrentProfileId();
            var followee = await profileFollowService.GetByFollowerIdAndFolloweeIdAsync(currentUserId.ToString(), profileId);
            if (followee == null)
            {
                return NotFound();
            }
            var result = await profileFollowService.UnFollow(followee.Id.ToString());
            return Ok(
                result
            );
        }

        [HttpPost("block")]
        [Authorize]
        public async Task<IActionResult> Block([FromBody] string followeeId)
        {
            Guid? currentUserId = GetCurrentProfileId();
            var followee = await profileFollowService.GetByFollowerIdAndFolloweeIdAsync(currentUserId.ToString(), followeeId);
            if (followee != null)
            {
                await profileFollowService.UnFollow(followee.Id.ToString());
            }
            var followee1 = await profileFollowService.GetByFollowerIdAndFolloweeIdAsync(followeeId, currentUserId.ToString());
            if (followee1 != null)
            {
                await profileFollowService.UnFollow(followee1.Id.ToString());
            }
            var friendShip = await friendshipService.HasFriendship(currentUserId.ToString(), followeeId);
            if (friendShip != null)
            {
                await friendshipService.Unfriend(friendShip.Id);
            }
            var blockResponse = await profileBlockService.Block(currentUserId.ToString(), followeeId);
            return Ok(blockResponse);
        }

        [HttpDelete("block/{profileId}")]
        [Authorize]
        public async Task<IActionResult> UnBlock(string profileId)
        {
            Guid? currentUserId = GetCurrentProfileId();
            var blockee = await profileBlockService.GetByBlockerIdAndBlockeeIdAsync(currentUserId.ToString(), profileId);
            if (blockee == null)
            {
                return NotFound();
            }

            var result = await profileBlockService.UnBlock(blockee.Id.ToString());
            return Ok(
                result
            );
        }
    }
}
