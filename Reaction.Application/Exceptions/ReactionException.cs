using System;

namespace InfinityNetServer.Services.Reaction.Application.Exceptions
{
    public class ReactionException : Exception
    {
        public ReactionErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public ReactionException(ReactionErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
