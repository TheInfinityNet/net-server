using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Identity.Application.Exceptions
{
    public sealed record IdentityErrorCode : BaseErrorCode
    {

        private IdentityErrorCode(string code, string message) : base(code, message) { }

        /*
        
        Chỗ này định nghĩa tất cả các lỗi CÓ LIÊN QUAN đến các chức năng/nghiệp vụ của service, mỗi loại lỗi gồm:
        - code: mã lỗi để frontend nhận biết, tạm thời cứ đặt ngẫu hứng, sau kết hợp vs frontend rồi refactor lại

        - message: ĐÂY LÀ KEY của message được định nghiã trong "Resources/SharedResource.[lang-code].resx", 
                    dùng để map với value của message tương ứng theo ngôn ngữ hiện tại trong Application Context.

         */

        // Static instances to represent each error code
        public static readonly IdentityErrorCode INVALID_EMAIL = new("auth/invalid-email", "invalid_email");
        public static readonly IdentityErrorCode WEAK_PASSWORD = new("auth/weak-password", "weak_password");
        public static readonly IdentityErrorCode PASSWORD_MISMATCH = new("auth/password-mismatch", "password_mismatch");
        public static readonly IdentityErrorCode TERMS_NOT_ACCEPTED = new("auth/terms-not-accepted", "terms_not_accepted");

        // Authentication Errors
        public static readonly IdentityErrorCode WRONG_PASSWORD = new("auth/wrong-password", "wrong_password");
        public static readonly IdentityErrorCode EXPIRED_PASSWORD = new("auth/expired-password", "expired_password");
        public static readonly IdentityErrorCode TWO_FACTOR_REQUIRED = new("auth/two-factor-required", "two_factor_required");
        public static readonly IdentityErrorCode INVALID_ACTIVATION_CODE = new("auth/invalid-activation-code", "invalid_activation_code");

        // Token Errors
        public static readonly IdentityErrorCode TOKEN_EXPIRED = new("auth/token-expired", "token_expired");
        public static readonly IdentityErrorCode INVALID_TOKEN = new("auth/invalid-token", "invalid_token");
        public static readonly IdentityErrorCode TOKEN_REVOKED = new("auth/token-revoked", "token_revoked");
        public static readonly IdentityErrorCode TOKEN_BLACKLISTED = new("auth/token-blacklisted", "token_blacklisted");

        // Verification Errors
        public static readonly IdentityErrorCode CODE_INVALID = new("auth/code-invalid", "code_invalid");

        // User Errors
        public static readonly IdentityErrorCode USER_DISABLED = new("auth/user-disabled", "user_disabled");
        public static readonly IdentityErrorCode USER_NOT_ACTIVATED = new("auth/user-not-activated", "user_not_activated");
        public static readonly IdentityErrorCode USER_NOT_FOUND = new("auth/user-not-found", "user_not_found");
        public static readonly IdentityErrorCode EMAIL_ALREADY_IN_USE = new("auth/email-already-in-use", "email_already_in_use");
        public static readonly IdentityErrorCode USERNAME_ALREADY_IN_USE = new("auth/username-already-in-use", "username_already_in_use");
        public static readonly IdentityErrorCode USER_ALREADY_VERIFIED = new("auth/user-already-verified", "user_already_verified");
        public static readonly IdentityErrorCode CANNOT_SEND_EMAIL = new("auth/cannot-send-email", "cannot_send_email");

    }

}
