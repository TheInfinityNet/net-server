using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Relationship.Application.Exceptions
{
    public sealed record RelationshipError : BaseError
    {

        private RelationshipError(ErrorType type, string code) : base(type, code) { }

        public static readonly RelationshipError FRIENDS_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.FriendsNotFound");

    }

}
