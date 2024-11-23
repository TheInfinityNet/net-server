using System;

namespace InfinityNetServer.Services.Notification.Application.Exceptions
{
    public class NotificationException(NotificationError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public NotificationError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
