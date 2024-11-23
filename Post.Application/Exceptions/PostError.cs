using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Post.Application.Exceptions
{
    public sealed record PostError : BaseError
    {

        private PostError(ErrorType type, string code) : base(type, code) { }

        public static readonly PostError INVALID_POST_TYPE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.InvalidPostType");

    }

}
