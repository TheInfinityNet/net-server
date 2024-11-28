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
    }
}
