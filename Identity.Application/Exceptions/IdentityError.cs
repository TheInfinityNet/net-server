using InfinityNetServer.BuildingBlocks.Application.Exceptions;

namespace InfinityNetServer.Services.Identity.Application.Exceptions
{
    public sealed record IdentityError : BaseError
    {

        private IdentityError(ErrorType type, string code) : base(type, code) { }

        // Static instances to represent each error code
        public static readonly IdentityError INVALID_EMAIL = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.InvalidEmail");
        public static readonly IdentityError WEAK_PASSWORD = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.WeakPassword");
        public static readonly IdentityError PASSWORD_MISMATCH = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.PasswordMismatch");
        public static readonly IdentityError TERMS_NOT_ACCEPTED = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.TermsNotAccepted");
        public static readonly IdentityError INVALID_BIRTHDATE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.InvalidBirthdate");
        public static readonly IdentityError INVALID_PROVIDER = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.InvalidProvider");

        // Authentication Errors
        public static readonly IdentityError WRONG_PASSWORD = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.WrongPassword");
        public static readonly IdentityError EXPIRED_PASSWORD = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.ExpiredPassword");
        public static readonly IdentityError TWO_FACTOR_REQUIRED = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.TwoFactorRequired");
        public static readonly IdentityError INVALID_ACTIVATION_CODE = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.InvalidActivationCode");

        // Token Errors
        public static readonly IdentityError TOKEN_EXPIRED = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.TokenExpired");
        public static readonly IdentityError INVALID_TOKEN = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.InvalidToken");
        public static readonly IdentityError TOKEN_REVOKED = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.TokenRevoked");
        public static readonly IdentityError TOKEN_BLACKLISTED = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.TokenBlacklisted");

        // Verification Errors
        public static readonly IdentityError CODE_INVALID = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.CodeInvalid");

        // User Errors
        public static readonly IdentityError EMAIL_ALREADY_IN_USE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.EmailAlreadyInUse");
        public static readonly IdentityError USERNAME_ALREADY_IN_USE = new(ErrorType.ValidationError, $"{ErrorType.ValidationError}.UsernameAlreadyInUse");
        public static readonly IdentityError CANNOT_SEND_EMAIL = new(ErrorType.UnExpected, $"{ErrorType.UnExpected}.CannotSendEmail");

        public static readonly IdentityError EMAIL_WAS_NOT_VERIFIED = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.EmailWasNotVerified");

    }

}
