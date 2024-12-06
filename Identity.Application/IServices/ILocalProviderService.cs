using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.IServices
{
    public interface ILocalProviderService
    {

        Task<LocalProvider> GetByEmail(string email);

    }
}
