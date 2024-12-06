using InfinityNetServer.Services.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.IServices
{
    public interface IVerificationService
    {

        public Task<Verification> Create(Verification verification);

        public Task<Verification> Delete(string id);

        public Task<Verification> GetById(string id);

        public Task<Verification> GetByCodeAndAccountId(string code, string accountId);

    }
}
