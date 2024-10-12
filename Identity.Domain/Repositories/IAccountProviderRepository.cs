using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Domain.Repositories
{
    public interface IAccountProviderRepository
    {
        Task CreateAccountProvidersAsync(IEnumerable<AccountProvider> accountProviders);

        Task<List<AccountProvider>> GetAllAccountProvidersAsync();

    }
}
