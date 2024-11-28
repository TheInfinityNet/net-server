using System;

namespace InfinityNetServer.BuildingBlocks.Application.Exceptions
{
    public class BaseException(BaseError error, int httpStatus, params object[] moreInfo) : Exception
    {

        public BaseError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
