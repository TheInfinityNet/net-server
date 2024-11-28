using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Post.Application.Exceptions
{
    public sealed record PostError : BaseError
    {

        private PostError(ErrorType type, string code) : base(type, code) { }

        public static readonly PostError INVALID_POST_TYPE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.InvalidPostType");

        public static readonly PostError INVALID_AUDIENCE_TYPE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.InvalidAudienceType");

        public static readonly PostError REQUIRED_PHOTO_ID = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.RequiredPhotoId");

        public static readonly PostError REQUIRED_VIDEO_ID = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.RequiredVideoId");

        public static readonly PostError REQUIRED_INCLUDE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.RequiredInclude");

        public static readonly PostError REQUIRED_EXCLUDE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.RequiredExclude");

        public static readonly PostError REQUIRED_INCLUDE_OR_EXCLUDE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.RequiredIncludeOrExclude");
    }

}
