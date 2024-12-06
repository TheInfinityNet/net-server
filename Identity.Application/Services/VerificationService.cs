using InfinityNetServer.Services.Identity.Application.IServices;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.Services
{
    public class VerificationService(
        ILogger<VerificationService> logger,
        IVerificationRepository verificationRepository) : IVerificationService
    {
        public async Task<Verification> Create(Verification verification)
        {
            logger.LogInformation("Creating verification code for account with id: {0}", verification.AccountId);
            return await verificationRepository.CreateAsync(verification);
        }

        public Task<Verification> Delete(string id)
        {
            logger.LogInformation("Deleting verification with id: {0}", id);
            return verificationRepository.DeleteAsync(Guid.Parse(id));
        }

        public async Task<Verification> GetByCodeAndAccountId(string code, string accountId)
            => await verificationRepository.GetByCodeAndAccountIdAsync(code, Guid.Parse(accountId));

        public async Task<Verification> GetById(string id)
            => await verificationRepository.GetByIdAsync(Guid.Parse(id));

    }
}
