using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using InfinityNetServer.BuildingBlocks.Application.Services;
using System;
using Microsoft.Extensions.Logging;

namespace InfinityNetServer.BuildingBlocks.Presentation.Services;

public class AuthenticatedUserService : IAuthenticatedUserService
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<AuthenticatedUserService> _logger;

    public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor, ILogger<AuthenticatedUserService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public bool IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public Guid? GetAuthenticatedUserId()
    {
        try
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity != null && user.Identity.IsAuthenticated)
            {
                return Guid.Parse(user.FindFirstValue("sub"));
            }
            return null;
        }
        catch (Exception)
        {
            _logger.LogError("Error getting authenticated user id");
            return null;
        }
    }

}
