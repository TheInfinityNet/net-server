using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Notification.Application;
using InfinityNetServer.Services.Notification.Application.DTOs;
using InfinityNetServer.Services.Notification.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Presentation.Controllers
{
    [ApiController]
    public class NotificationController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<NotificationController> logger,
        IStringLocalizer<NotificationSharedResource> localizer,
        IMapper mapper,
        INotificationService notificationService,
        CommonFileClient fileClient) : BaseApiController(authenticatedUserService)
    {

        [Authorize]
        [EndpointDescription("Get notifications")]
        [HttpGet("{cursor}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetNotifications(string cursor = null)
        {
            var notifications = await notificationService.GetNewestNotifications(
                authenticatedUserService.GetAuthenticatedAccountId().Value.ToString(), cursor);
            BCursorPagedResult<NotificationResponse> response = new()
            {
                Items = notifications.Items.Select(n => {
                    switch (n.Type)
                    {
                        case BuildingBlocks.Domain.Enums.NotificationType.FriendInvitation:
                            n.Title = localizer["FriendInvitationNotification.Title"];
                            n.Content = localizer["FriendInvitationNotification.Content", n.Content];
                            break;
                    }
                    return mapper.Map<NotificationResponse>(n);
                    }).ToList(),
                NextCursor = notifications.NextCursor,
                PrevCursor = notifications.PrevCursor
            };
            return Ok(response);
        }

    }
}
