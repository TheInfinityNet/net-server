using System;

namespace InfinityNetServer.Services.Tag.Application.Exceptions
{
    public class TagException : Exception
    {
        public TagErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public TagException(TagErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
