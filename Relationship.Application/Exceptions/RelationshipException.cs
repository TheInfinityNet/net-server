using System;

namespace InfinityNetServer.Services.Relationship.Application.Exceptions
{
    public class RelationshipException(RelationshipError error, int httpStatus, params object[] moreInfo) : Exception
    {
        public RelationshipError Error { get; }

        public int HttpStatus { get; }

        public object[] MoreInfo { get; }

    }
}
