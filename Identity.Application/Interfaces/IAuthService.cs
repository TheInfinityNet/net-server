using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.Interfaces
{
    public interface IAuthService
    {

        Task<Account> SignIn(string email, string password);

        Task<bool> Introspect(string token);

        string GenerateToken(Account user, bool isRefresh);

        Task<string> Refresh(string refreshToken, string accessToken);

    }
}
