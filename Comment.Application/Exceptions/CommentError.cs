using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Comment.Application.Exceptions
{
    public sealed record CommentError : BaseError
    {

        private CommentError(ErrorType type, string code) : base(type, code) { }

        public static readonly CommentError REQUIRED_PHOTO_ID = new(ErrorType.ValidationError, "Comment.RequiredPhotoId");

        public static readonly CommentError REQUIRED_VIDEO_ID = new(ErrorType.ValidationError, "Comment.RequiredVideoId");

        public static readonly CommentError INVALID_COMMENT_TYPE = new(ErrorType.ValidationError, "Comment.InvalidCommentType");

    }

}
