using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using InfinityNetServer.BuildingBlocks.Application.Interfaces;

namespace InfinityNetServer.BuildingBlocks.Presentation.Services.AuthenticatedUser;

public class AuthenticatedUserService : IAuthenticatedUserService
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public string GetAuthenticatedUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity != null && user.Identity.IsAuthenticated)
        {
            return user.FindFirstValue("sub") ?? "anonymous";
        }
        return "anonymous";
    }

}
