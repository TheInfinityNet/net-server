using System;

namespace InfinityNetServer.Services.Comment.Application.Exceptions
{
    public class CommentException(CommentError error, int httpStatus, params object[] moreInfo) : Exception
    {

        public CommentError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
