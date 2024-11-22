using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.File.Application.Exceptions
{
    public sealed record FileError : BaseError
    {

        private FileError(ErrorType type, string code) : base(type, code) { }

        public static readonly FileError CAN_NOT_STORE_FILE = new(ErrorType.UnExpected, $"{ErrorType.UnExpected}.CanNotStoreFile");

        public static readonly FileError CAN_NOT_DELETE_FILE = new(ErrorType.UnExpected, $"{ErrorType.UnExpected}.CanNotDeleteFile");

        public static readonly FileError CAN_NOT_RETRIEVE_FILE = new(ErrorType.UnExpected, $"{ErrorType.UnExpected}.CanNotRetrieveFile");

        public static readonly FileError BUCKET_CREATION_FAILED = new(ErrorType.UnExpected, $"{ErrorType.UnExpected}.BucketCreationFailed");

        public static readonly FileError CAN_NOT_COPY_FILE = new(ErrorType.UnExpected, $"{ErrorType.UnExpected}.CanNotCopyFile");

        public static readonly FileError FILE_EMPTY = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.FileEmpty");

        public static readonly FileError FILE_SIZE_EXCEEDED = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.FileSizeExceeded");

        public static readonly FileError INVALID_FILE_TYPE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.InvalidFileType");

    }

}
