using System;

namespace InfinityNetServer.Services.Group.Application.Exceptions
{
    public class GroupException(GroupError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public GroupError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
