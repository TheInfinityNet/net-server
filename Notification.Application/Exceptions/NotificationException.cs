using System;

namespace InfinityNetServer.Services.Notification.Application.Exceptions
{
    public class NotificationException(NotificationError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public NotificationError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
