using System;

namespace InfinityNetServer.Services.Post.Application.Exceptions
{
    public class PostException(PostError error, int httpStatus, params object[] moreInfo) : Exception
    {

        public PostError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
