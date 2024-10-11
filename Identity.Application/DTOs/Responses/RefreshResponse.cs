namespace InfinityNetServer.Services.Identity.Application.DTOs.Responses
{
    public sealed record RefreshResponse(

        string Message,

        string AccessToken

    );
}
