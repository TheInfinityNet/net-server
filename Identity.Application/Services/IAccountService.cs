using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.Services
{
    public interface IAccountService
    {

        Task<Account> GetById(string id);

    }
}
