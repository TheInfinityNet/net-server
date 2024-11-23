using System;

namespace InfinityNetServer.Services.Profile.Application.Exceptions
{
    public class ProfileException(ProfileError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public ProfileError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
