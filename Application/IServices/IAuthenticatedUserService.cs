using System;

namespace InfinityNetServer.BuildingBlocks.Application.IServices;

public interface IAuthenticatedUserService
{

    public bool IsAuthenticated();

    public Guid? GetAuthenticatedProfileId();

    public Guid? GetAuthenticatedAccountId();
}
