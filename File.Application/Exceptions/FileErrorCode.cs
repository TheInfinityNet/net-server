using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.File.Application.Exceptions
{
    public sealed record FileErrorCode : BaseErrorCode
    {

        private FileErrorCode(string code, string message) : base(code, message) { }

        /*
        
        Chỗ này định nghĩa tất cả các lỗi CÓ LIÊN QUAN đến các chức năng/nghiệp vụ của service, mỗi loại lỗi gồm:
        - code: mã lỗi để frontend nhận biết, tạm thời cứ đặt ngẫu hứng, sau kết hợp vs frontend rồi refactor lại

        - message: ĐÂY LÀ KEY của message được định nghiã trong "Resources/SharedResource.[lang-code].resx", 
                    dùng để map với value của message tương ứng theo ngôn ngữ hiện tại trong Application Context.

         */

        // Static instances to represent each error code
        public static readonly FileErrorCode CAN_NOT_STORE_FILE = new("file/can-not-store-file", "can_not_store_file");
        public static readonly FileErrorCode CAN_NOT_DELETE_FILE = new("file/can-not-delete-file", "can_not_delete_file");
        public static readonly FileErrorCode BUCKET_CREATION_FAILED = new("file/bucket-creation-failed", "bucket_creation_failed");
        public static readonly FileErrorCode FILE_EMPTY = new("file/file-empty", "file_empty");
        public static readonly FileErrorCode FILE_SIZE_EXCEEDED = new("file/file-size-exceeded", "file_size_exceeded");
        public static readonly FileErrorCode INVALID_FILE_TYPE = new("file/invalid-file-type", "invalid_file_type");

    }

}
