using InfinityNetServer.BuildingBlocks.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Controllers;

public abstract class BaseApiController(IAuthenticatedUserService authenticatedUserService) : ControllerBase
{

    protected Guid? GetCurrentAccountId() => authenticatedUserService.GetAuthenticatedAccountId();

    protected Guid? GetCurrentProfileId() => authenticatedUserService.GetAuthenticatedProfileId();

}
