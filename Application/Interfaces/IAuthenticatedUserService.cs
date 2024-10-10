namespace InfinityNetServer.BuildingBlocks.Application.Interfaces;

public interface IAuthenticatedUserService
{

    bool IsAuthenticated();

    string GetAuthenticatedUserId();

}
