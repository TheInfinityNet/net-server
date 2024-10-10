using System;

namespace InfinityNetServer.BuildingBlocks.Application.Exceptions
{
    public class CommonException : Exception
    {
        public BaseErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public CommonException(BaseErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
