using System;

namespace InfinityNetServer.Services.Identity.Application.Exceptions
{
    public class IdentityException : Exception
    {
        public IdentityErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public IdentityException(IdentityErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
