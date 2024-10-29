using System.Collections.Generic;
using InfinityNetServer.BuildingBlocks.Application.Bus;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Relationship.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses;
using System.Linq;
using InfinityNetServer.Services.Relationship.Application.GrpcClients;

namespace InfinityNetServer.Services.Relationship.Presentation.Controllers
{
    [ApiController]
    [Route("friendship")]
    public class TestController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<TestController> logger,
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

        [HttpGet("friends/preview/{profileId}")]
        public async Task<IActionResult> GetFriends(string profileId)
        {
            IList<string> friendIds = await friendshipService.GetPreviewFriendIds(profileId);
            IList<UserProfileResponse> friends = await profileClient.GetFriendsOfProfile(friendIds);
            int totalFriends = await friendshipService.CountFriendships(profileId);
            return Ok(new
            {
                friends,
                totalFriends
            });
        }

    }
}
