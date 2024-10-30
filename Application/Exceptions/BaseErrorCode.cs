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
    public static readonly BaseErrorCode VALIDATION_ERROR = new("common/validation-error", "validation_error");
    public static readonly BaseErrorCode SEED_DATA_ERROR = new("common/seed-data-error", "seed_data_error");

    // Token Errors
    public static readonly BaseErrorCode TOKEN_MISSING = new("common/token-missing", "token_missing");
    public static readonly BaseErrorCode TOKEN_INVALID = new("common/token-invalid", "token_invalid");
    public static readonly BaseErrorCode INVALID_SIGNATURE = new("common/invalid-signature", "invalid_signature");

    // Rate Limiting Errors
    public static readonly BaseErrorCode TOO_MANY_REQUESTS = new("common/too-many-requests", "too_many_requests");
    public static readonly BaseErrorCode RATE_LIMIT_EXCEEDED = new("common/rate-limit-exceeded", "rate_limit_exceeded");

    // Relationship Errors
    public static readonly BaseErrorCode RELATIONSHIP_NOT_FOUND = new("common/relationship-not-found", "relationship_not_found");

}
