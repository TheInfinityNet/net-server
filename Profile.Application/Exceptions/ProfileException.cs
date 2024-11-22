using System;

namespace InfinityNetServer.Services.Profile.Application.Exceptions
{
    public class ProfileException(ProfileError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public ProfileError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
