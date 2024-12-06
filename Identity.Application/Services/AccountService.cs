using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.IServices;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.Services
{
    public class AccountService(
            IAccountRepository accountRepository,
            ILogger<AccountService> logger,
            IStringLocalizer<IdentitySharedResource> localizer) : IAccountService
    {

        public async Task<Account> GetById(string id)
            => await accountRepository.GetByIdAsync(Guid.Parse(id));

        public async Task<Account> Create(Account account)
        {
            logger.LogInformation(account.DefaultUserProfileId.ToString());
            return await accountRepository.CreateAsync(account);
        }
    }
}
