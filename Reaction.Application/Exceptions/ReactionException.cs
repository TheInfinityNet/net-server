using System;

namespace InfinityNetServer.Services.Reaction.Application.Exceptions
{
    public class ReactionException(ReactionError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public ReactionError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
