﻿using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Relationship.Application.Exceptions
{
    public sealed record RelationshipErrorCode : BaseErrorCode
    {

        private RelationshipErrorCode(string code, string message) : base(code, message) { }

        /*
        
        Chỗ này định nghĩa tất cả các lỗi CÓ LIÊN QUAN đến các chức năng/nghiệp vụ của service, mỗi loại lỗi gồm:
        - code: mã lỗi để frontend nhận biết, tạm thời cứ đặt ngẫu hứng, sau kết hợp vs frontend rồi refactor lại

        - message: ĐÂY LÀ KEY của message được định nghiã trong "Resources/SharedResource.[lang-code].resx", 
                    dùng để map với value của message tương ứng theo ngôn ngữ hiện tại trong Application Context.

         */

        // Static instances to represent each error code
        public static readonly RelationshipErrorCode FRIENDS_NOT_FOUND = new("relationship/friends-not-found", "friends_not_found");

    }

}
