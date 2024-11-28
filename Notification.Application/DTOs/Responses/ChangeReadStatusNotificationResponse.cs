using System;

namespace InfinityNetServer.Services.Notification.Application.DTOs.Responses
{
    public class ChangeReadStatusNotificationResponse
    {
        public Guid Id { get; set; }

        public bool IsRead { get; set; }

        public string Message { get; set; }
    }
}
