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
            var notifications = await notificationService
                .GetNewestNotifications(GetCurrentAccountId().Value.ToString(), cursor);

            BCursorPagedResult<NotificationResponse> response = new()
            {
                Items = notifications.Items.Select(n => {
                    var response = mapper.Map<NotificationResponse>(n);
                    response.Title = localizer[$"{n.Type}.Title", n.TitleParams];
                    response.Content = localizer[$"{n.Type}.Content", n.ContentParams];
                    return response;
                }).ToList(),
                NextCursor = notifications.NextCursor,
                PrevCursor = notifications.PrevCursor
            };
            return Ok(response);
        }

    }
}
