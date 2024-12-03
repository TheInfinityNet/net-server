using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Reaction.Application.Exceptions
{
    public sealed record ReactionError : BaseError
    {

        private ReactionError(ErrorType type, string code) : base(type, code) { }

        public static readonly ReactionError CREATE_REACTION_FAILED = new(ErrorType.UnExpected, $"{ErrorType.UnExpected}.CreateReactionFailed");

        public static readonly ReactionError DELETE_REACTION_FAILED = new(ErrorType.UnExpected, $"{ErrorType.UnExpected}.DeleteReactionFailed");
    }

}
