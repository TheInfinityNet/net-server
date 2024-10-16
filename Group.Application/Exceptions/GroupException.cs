using System;

namespace InfinityNetServer.Services.Group.Application.Exceptions
{
    public class GroupException : Exception
    {
        public GroupErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public GroupException(GroupErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
