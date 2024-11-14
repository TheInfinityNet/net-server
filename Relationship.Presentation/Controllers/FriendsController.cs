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

namespace InfinityNetServer.Services.Relationship.Presentation.Controllers
{
    [ApiController]
    [Route("friends")]
    public class FriendsController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<FriendsController> logger,
        IStringLocalizer<RelationshipSharedResource> Localizer,
        IMessageBus messageBus,
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
        [HttpGet("suggestions")]
        [Authorize]
        public async Task<IActionResult> GetFriendSuggestions([FromQuery] Guid? nextCursor, [FromQuery] int limit = 10)
        {
            Guid? currentUserId = GetCurrentUserId();
            
            var suggestions = await friendshipService.GetPagedCommonFriendsAsync(currentUserId, nextCursor, limit);
            var result = profileClient.GetPreviewFriendsOfProfile(suggestions.Results.Select(guid => guid.ToString()).ToList());
            return Ok(new
            {
                result
            });
        }
    }
}
