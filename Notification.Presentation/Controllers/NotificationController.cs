using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Notification.Application;
using InfinityNetServer.Services.Notification.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace InfinityNetServer.Services.Notification.Presentation.Controllers
{
    [ApiController]
    public class NotificationController(
        IAuthenticatedUserService authenticatedUserService,
        ILogger<NotificationController> logger,
        IStringLocalizer<NotificationSharedResource> Localizer,
        IMessageBus messageBus,
        INotificationService photoMetadataService,
        CommonPostClient postClient,
        CommonCommentClient commentClient) : BaseApiController(authenticatedUserService)
    {

        
    }
}
