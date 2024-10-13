using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.Interfaces
{
    public interface IAccountService
    {

        Task<Account> GetAccountById(string id);

        Task<Account> GetAccountByEmail(string email);

    }
}
