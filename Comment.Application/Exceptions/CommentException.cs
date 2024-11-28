using System;

namespace InfinityNetServer.Services.Comment.Application.Exceptions
{
    public class CommentException(CommentError error, int httpStatus, params object[] moreInfo) : Exception
    {

        public CommentError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
