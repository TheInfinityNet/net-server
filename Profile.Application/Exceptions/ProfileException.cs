using System;

namespace InfinityNetServer.Services.Profile.Application.Exceptions
{
    public class ProfileException : Exception
    {
        public ProfileErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public ProfileException(ProfileErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
