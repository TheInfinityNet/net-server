using System;

namespace InfinityNetServer.Services.Relationship.Application.Exceptions
{
    public class RelationshipException(RelationshipError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public RelationshipError Error { get; } = error;

        public int HttpStatus { get; } = httpStatus;

        public object[] MoreInfo { get; } = moreInfo;

    }
}
