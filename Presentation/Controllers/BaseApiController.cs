using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfinityNetServer.BuildingBlocks.Presentation.Controllers;

public abstract class BaseApiController : ControllerBase
{

    private readonly IAuthenticatedUserService _authenticatedUserService;

    public BaseApiController(IAuthenticatedUserService authenticatedUserService)
    {
        _authenticatedUserService = authenticatedUserService;
    }

    protected string GetCurrentUserId()
    {
        return _authenticatedUserService.GetAuthenticatedUserId();
    }

}
