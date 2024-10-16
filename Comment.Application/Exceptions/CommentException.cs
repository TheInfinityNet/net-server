using System;

namespace InfinityNetServer.Services.Comment.Application.Exceptions
{
    public class CommentException : Exception
    {
        public CommentErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public CommentException(CommentErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
