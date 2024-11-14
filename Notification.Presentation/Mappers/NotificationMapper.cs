using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Notification.Application;
using InfinityNetServer.Services.Notification.Application.DTOs;
using Microsoft.Extensions.Localization;
using System;

namespace InfinityNetServer.Services.Notification.Presentation.Mappers;

public class NotificationMapper : AutoMapper.Profile
{
    public NotificationMapper(IStringLocalizer<NotificationSharedResource> localizer)
    {

        CreateMap<Domain.Entities.Notification, NotificationResponse>()
            .AfterMap((src, dest) =>
            {
                if (src.ThumbnailId == null)
                    dest.Thumbnail = new PhotoMetadataResponse
                    {
                        Id = Guid.Empty,
                        Name = "cover.jpg",
                        Width = 500,
                        Height = 500,
                        Size = 1000,
                        Type = FileMetadataType.Photo.ToString(),
                        Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRmCy16nhIbV3pI1qLYHMJKwbH2458oiC9EmA&s",
                        CreatedAt = DateTime.Now
                    };
                else
                    dest.Thumbnail = new PhotoMetadataResponse
                    {
                        Id = src.ThumbnailId.Value,
                    };

                dest.Type = src.Type.ToString();
                dest.Title = localizer["TaggedInCommentNotification.Title", src.Title];
                dest.Content = localizer["TaggedInCommentNotification.Content", src.Content];
            });
    }
}
