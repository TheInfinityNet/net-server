using System;

namespace InfinityNetServer.Services.Relationship.Application.Exceptions
{
    public class RelationshipException : Exception
    {
        public RelationshipErrorCode ErrorCode { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

        public RelationshipException(RelationshipErrorCode errorCode, int httpStatus, params object[] moreInfo) : base()
        {
            ErrorCode = errorCode;
            HttpStatus = httpStatus;
            MoreInfo = moreInfo;
        }

    }
}
