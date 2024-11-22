using System;

namespace InfinityNetServer.Services.Group.Application.Exceptions
{
    public class GroupException(GroupError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public GroupError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
