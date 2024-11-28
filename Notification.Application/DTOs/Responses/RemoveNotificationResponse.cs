using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Notification.Application.DTOs.Responses
{
    public class RemoveNotificationResponse
    {
        public Guid Id { get; set; }

        public string Message { get; set; }
    }
}
