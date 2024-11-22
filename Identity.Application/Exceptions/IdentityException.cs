using System;

namespace InfinityNetServer.Services.Identity.Application.Exceptions
{
    public class IdentityException(IdentityError errorCode, int httpStatus, params object[] moreInfo) : Exception
    {

        public IdentityError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
