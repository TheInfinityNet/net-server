using System;

namespace InfinityNetServer.Services.Notification.Application.Exceptions
{
    public class NotificationException : Exception
    {
        public NotificationErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public NotificationException(NotificationErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
