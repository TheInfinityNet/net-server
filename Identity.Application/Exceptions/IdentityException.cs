using System;

namespace InfinityNetServer.Services.Identity.Application.Exceptions
{
    public class IdentityException(IdentityError error, int httpStatus, params object[] moreInfo) : Exception
    {

        public IdentityError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
