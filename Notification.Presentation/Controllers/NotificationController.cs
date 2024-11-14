using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Notification.Application;
using InfinityNetServer.Services.Notification.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Presentation.Controllers
{
    [ApiController]
    public class NotificationController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<NotificationController> logger,
        IStringLocalizer<NotificationSharedResource> Localizer,
        INotificationService notificationService,
        CommonFileClient fileClient) : BaseApiController(authenticatedUserService)
    {

        [EndpointDescription("Get notifications")]
        [HttpGet("{accountId}/{cursor}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetNotifications(string accountId, string cursor = null)
        {
            var notifications = await notificationService.GetNewestNotifications(accountId, cursor);
            return Ok(notifications);
        }

    }
}
