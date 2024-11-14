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

    public Guid? GetAuthenticatedProfileId()
    {
        try
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user == null) return null;

            if (user.Identity != null & user.Identity!.IsAuthenticated) 
                return Guid.Parse(user.FindFirstValue("profile_id")!);
            
            return null;
        }
        catch (Exception)
        {
            logger.LogError("Error getting authenticated profile id");
            return null;
        }
    }

    public Guid? GetAuthenticatedAccountId()
    {
        try
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user == null) return null;

            if (user.Identity != null & user.Identity!.IsAuthenticated)
                return Guid.Parse(user.FindFirstValue("sub")!);

            return null;
        }
        catch (Exception)
        {
            logger.LogError("Error getting authenticated account id");
            return null;
        }
    }

}
