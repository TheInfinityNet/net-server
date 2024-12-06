using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.IServices
{
    public interface IAccountService
    {

        public Task<Account> GetById(string id);

        public Task<Account> Create(Account account);

    }
}
