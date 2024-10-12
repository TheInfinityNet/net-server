using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Domain.Repositories
{
    public interface IVerificationRepository
    {

        Task CreateVerificationsAsync(IEnumerable<Verification> verifications);

        Task<List<Verification>> GetAllVerificationsAsync();

    }
}
