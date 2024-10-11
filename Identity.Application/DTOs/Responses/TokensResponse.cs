namespace InfinityNetServer.Services.Identity.Application.DTOs.Responses
{
    public sealed record TokensResponse(
        string AccessToken,
        string RefreshToken
    );
}
