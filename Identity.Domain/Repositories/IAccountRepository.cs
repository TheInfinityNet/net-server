using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Domain.Repositories
{
    public interface IAccountRepository
    {

        Task CreateAccountsAsync(IEnumerable<Account> accounts);

        Task<List<string>> GetAllAccountIdsAsync();

        Task<List<Account>> GetAllAccountsAsync();

        Task<Account> CreateAccountAsync(Account account);

        Task UpdateAccountAsync(Account account);

    }
}
