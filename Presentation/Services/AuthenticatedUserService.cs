using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using InfinityNetServer.BuildingBlocks.Application.Services;
using System;
using Microsoft.Extensions.Logging;

namespace InfinityNetServer.BuildingBlocks.Presentation.Services;

public class AuthenticatedUserService
    (IHttpContextAccessor httpContextAccessor, ILogger<AuthenticatedUserService> logger) : IAuthenticatedUserService
{

    public bool IsAuthenticated()
    {
        return httpContextAccessor!.HttpContext!.User!.Identity!.IsAuthenticated;
    }

    public Guid? GetAuthenticatedUserId()
    {
        try
        {
            var user = httpContextAccessor.HttpContext!.User;
            if (user.Identity != null & user.Identity!.IsAuthenticated) 
                return Guid.Parse(user.FindFirstValue("profile_id")!);
            
            return null;
        }
        catch (Exception)
        {
            logger.LogError("Error getting authenticated user id");
            return null;
        }
    }

}
