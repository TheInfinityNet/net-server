using System;

namespace InfinityNetServer.Services.Post.Application.Exceptions
{
    public class PostException : Exception
    {
        public PostErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public PostException(PostErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
