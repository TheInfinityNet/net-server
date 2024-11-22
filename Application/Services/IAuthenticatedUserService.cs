using System;

namespace InfinityNetServer.BuildingBlocks.Application.Services;

public interface IAuthenticatedUserService
{

    public bool IsAuthenticated();

    public Guid? GetAuthenticatedProfileId();

    public Guid? GetAuthenticatedAccountId();
}
