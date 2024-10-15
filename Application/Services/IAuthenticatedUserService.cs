namespace InfinityNetServer.BuildingBlocks.Application.Services;

public interface IAuthenticatedUserService
{

    bool IsAuthenticated();

    string GetAuthenticatedUserId();

}
