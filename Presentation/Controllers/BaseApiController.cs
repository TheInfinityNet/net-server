using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Controllers;

public abstract class BaseApiController(IAuthenticatedUserService authenticatedUserService) : ControllerBase
{

    protected Guid GetCurrentAccountId()
    {
        return authenticatedUserService.GetAuthenticatedAccountId() 
            ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);
    }

    protected Guid GetCurrentProfileId()
    {
        return authenticatedUserService.GetAuthenticatedProfileId() 
            ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound); 
    }

    protected bool IsOwner(string profileId)
    {
        if (Guid.TryParse(profileId, out Guid id))
        {
            return id == GetCurrentProfileId();
        }
        return false;
    }

}
