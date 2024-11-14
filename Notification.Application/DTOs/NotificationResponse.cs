using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using System;

namespace InfinityNetServer.Services.Notification.Application.DTOs
{
    public class NotificationResponse
    {

        public Guid Id { get; set; }

        public string Type { get; set; }

        public PhotoMetadataResponse Thumbnail { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}
