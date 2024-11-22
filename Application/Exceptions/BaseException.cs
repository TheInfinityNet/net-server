using System;

namespace InfinityNetServer.BuildingBlocks.Application.Exceptions
{
    public class BaseException(BaseError error, int httpStatus, params object[] moreInfo) : Exception
    {

        public BaseError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
