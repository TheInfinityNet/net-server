namespace InfinityNetServer.BuildingBlocks.Application.Exceptions;

public record BaseError
{

    public ErrorType Type { get; protected set; }

    public string Code { get; protected set; }

    protected BaseError(ErrorType type, string code)
    {
        Type = type;
        Code = code;
    }

    // Static instances to represent each error code
    public static readonly BaseError VALIDATION_ERROR = new(ErrorType.ValidationError, ErrorType.ValidationError.ToString());

    // Token Errors
    public static readonly BaseError TOKEN_MISSING = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.TokenMissing");
    public static readonly BaseError TOKEN_INVALID = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.TokenInvalid");
    public static readonly BaseError INVALID_SIGNATURE = new(ErrorType.Unauthorized, $"{ErrorType.Unauthorized}.InvalidSignature");
    public static readonly BaseError NOT_HAVE_PERMISSION = new(ErrorType.Forbidden, $"{ErrorType.Forbidden}.NotHavePermission");

    // Rate Limiting Errors
    public static readonly BaseError TOO_MANY_REQUESTS = new(ErrorType.Forbidden, $"{ErrorType.Forbidden}.TooManyRequests");
    public static readonly BaseError RATE_LIMIT_EXCEEDED = new(ErrorType.Forbidden, $"{ErrorType.Forbidden}.RateLimitExceeded");

    // Not found Errors
    public static readonly BaseError ACCOUNT_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Account");
    public static readonly BaseError PROFILE_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Profile");
    public static readonly BaseError RELATIONSHIP_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Relationship");
    public static readonly BaseError POST_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Post");
    public static readonly BaseError COMMENT_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Comment");
    public static readonly BaseError REACTION_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Reaction");
    public static readonly BaseError FILE_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.File");
    public static readonly BaseError NOTIFICATION_NOT_FOUND = new(ErrorType.ResourceNotFound, $"{ErrorType.ResourceNotFound}.Notification");

}
