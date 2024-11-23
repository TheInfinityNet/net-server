using System;

namespace InfinityNetServer.Services.Reaction.Application.Exceptions
{
    public class ReactionException(ReactionError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public ReactionError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
