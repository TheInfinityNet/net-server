using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.Services.Notification.Application;
using InfinityNetServer.Services.Notification.Application.DTOs.Responses;
using InfinityNetServer.Services.Notification.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
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
        [HttpGet("/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetReadOrMentionNotifications(string cursor = null, int limit = 10, string type = "All")
        {
            var notifications = await notificationService
                .GetNewestUnreadOrMentionNotifications(GetCurrentAccountId().ToString(), cursor, limit, type);

            // chỗ này là duyệt kết quả truy vấn notification xong map sang list các photo id cần lấy
            var photoMetadataIds = notifications.Items
                .Where(noti => noti.ThumbnailId != null).Select(noti => noti.ThumbnailId).Distinct().ToList();

            // tạo 1 list các task bất đồng bộ để call grpc sang file service
            var photoMetadataTasks = photoMetadataIds.Select(async id =>
            {
                // dòng này call lấy về dto của photo khớp với frontend
                PhotoMetadataResponse metadata = await fileClient.GetPhotoMetadata(id.ToString());

                // dòng này nghĩa là nếu trong db của file có tồn tại file thì lấy metadata
                // không tồn tại thì chỉ lấy mỗi id
                return new { Id = id, Metadata = metadata ??= new PhotoMetadataResponse { Id = id.Value } };
            });

            // sau khi list các task bất đồng bộ call grpc có kết quả hết thì chuyển sang dictionary dạng key-value
            // { key: photoId, value: PhotoMetadataResponse }
            var photoMetadataDict = (await Task.WhenAll(photoMetadataTasks)).ToDictionary(x => x.Id, x => x.Metadata);

            CursorPagedResult<NotificationResponse> response = new()
            {
                Items = notifications.Items.Select(noti =>
                {
                    var response = mapper.Map<NotificationResponse>(noti);
                    response.Title = localizer[$"{noti.Type}.Title", noti.TitleParams];
                    response.Content = localizer[$"{noti.Type}.Content", noti.ContentParams];
                    response.Permalink = string.Empty;

                    // chỗ này duyệt để map kết quả sang dto thì đến lúc map cái thumnail 
                    // dùng thumnailId để lấy giá trị trong dictionary rồi xuất value ra biến "photo"
                    if (noti.ThumbnailId != null)
                    {
                        if (photoMetadataDict.TryGetValue(noti.ThumbnailId, out var photo))
                            // lấy đc là gán vô dto
                            response.Thumbnail = photo;
                    }

                    return response;
                }).ToList(),
                NextCursor = notifications.NextCursor,
                PrevCursor = notifications.PrevCursor
            };
            return Ok(response);
        }

        [Authorize]
        [EndpointDescription("Change Read Status Notification")]
        [HttpPut("update/{notificationId}")] // update xài put
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ChangeReadStatusNotification(Guid notificationId)
        {
            var notification = await notificationService.ChangeReadStatusNotification(notificationId) 
                ?? throw new BaseException(BaseError.NOTIFICATION_NOT_FOUND, StatusCodes.Status404NotFound);
            var dto = mapper.Map<ChangeReadStatusNotificationResponse>(notification);
            dto.Message = localizer["Message.ChangeStatusSuccess"];
            return Ok(dto);
        }

        [Authorize]
        [EndpointDescription("Remove a Notification")]
        [HttpDelete("{notificationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RemoveNotification(Guid notificationId)
        {
            Console.WriteLine(notificationId);
            var notification = await notificationService.RemoveNotification(notificationId) 
                ?? throw new BaseException(BaseError.NOTIFICATION_NOT_FOUND, StatusCodes.Status404NotFound);
            var dto = mapper.Map<RemoveNotificationResponse>(notification);
            dto.Message = localizer["Message.RemoveNotificationSuccess"];
            return Ok(dto);
        }
    }
}
