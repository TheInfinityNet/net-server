using InfinityNetServer.Services.Identity.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.Services
{
    public interface IAuthService
    {

        Task<LocalProvider> SignIn(string email, string password);

        Task<bool> Introspect(string token);

        string GenerateToken(Account account, Guid profileId, bool isRefresh);

        Task<string> Refresh(string refreshToken);

    }
}
