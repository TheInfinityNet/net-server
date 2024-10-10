namespace InfinityNetServer.BuildingBlocks.Application.Exceptions;

public record BaseErrorCode
{

    public string Code { get; protected set; }
    public string Message { get; protected set; }

    protected BaseErrorCode(string code, string message)
    {
        Code = code;
        Message = message;
    }

    // Static instances to represent each error code
    public static readonly BaseErrorCode VALIDATION_ERROR = new("auth/validation-error", "validation_error");

    // Token Errors
    public static readonly BaseErrorCode TOKEN_MISSING = new("common/token-missing", "token_missing");
    public static readonly BaseErrorCode TOKEN_INVALID = new("common/token-invalid", "token_invalid");
    public static readonly BaseErrorCode TOKEN_EXPIRED = new("auth/token-expired", "token_expired");
    public static readonly BaseErrorCode INVALID_TOKEN = new("auth/invalid-token", "invalid_token");
    public static readonly BaseErrorCode TOKEN_REVOKED = new("auth/token-revoked", "token_revoked");
    public static readonly BaseErrorCode TOKEN_BLACKLISTED = new("auth/token-blacklisted", "token_blacklisted");
    public static readonly BaseErrorCode INVALID_SIGNATURE = new("auth/invalid-signature", "invalid_signature");

    // Rate Limiting Errors
    public static readonly BaseErrorCode TOO_MANY_REQUESTS = new("auth/too-many-requests", "too_many_requests");
    public static readonly BaseErrorCode RATE_LIMIT_EXCEEDED = new("auth/rate-limit-exceeded", "rate_limit_exceeded");

}
