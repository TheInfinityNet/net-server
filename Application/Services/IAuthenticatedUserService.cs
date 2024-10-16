using System;

namespace InfinityNetServer.BuildingBlocks.Application.Services;

public interface IAuthenticatedUserService
{

    bool IsAuthenticated();

    Guid GetAuthenticatedUserId();

}
