using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Notification.Application.Exceptions
{
    public sealed record NotificationErrorCode : BaseErrorCode
    {

        private NotificationErrorCode(string code, string message) : base(code, message) { }

        /*
        
        Chỗ này định nghĩa tất cả các lỗi CÓ LIÊN QUAN đến các chức năng/nghiệp vụ của service, mỗi loại lỗi gồm:
        - code: mã lỗi để frontend nhận biết, tạm thời cứ đặt ngẫu hứng, sau kết hợp vs frontend rồi refactor lại

        - message: ĐÂY LÀ KEY của message được định nghiã trong "Resources/SharedResource.[lang-code].resx", 
                    dùng để map với value của message tương ứng theo ngôn ngữ hiện tại trong Application Context.

         */

        // Static instances to represent each error code

    }

}
