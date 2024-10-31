using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Responses
{
    public sealed record SignInResponse
    (

        TokensResponse Tokens,

        UserProfileResponse User

    );
}
