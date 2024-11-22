using System;

namespace InfinityNetServer.Services.Post.Application.Exceptions
{
    public class PostException(PostError error, int httpStatus, params object[] moreInfo) : Exception
    {

        public PostError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
