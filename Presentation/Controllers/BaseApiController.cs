using InfinityNetServer.BuildingBlocks.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InfinityNetServer.BuildingBlocks.Presentation.Controllers;

public abstract class BaseApiController : ControllerBase
{

    private readonly IAuthenticatedUserService _authenticatedUserService;

    public BaseApiController(IAuthenticatedUserService authenticatedUserService)
    {
        _authenticatedUserService = authenticatedUserService;
    }

    protected Guid GetCurrentUserId()
    {
        return _authenticatedUserService.GetAuthenticatedUserId();
    }

}
