using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.Services;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Presentation.Services
{
    public class AccountService : IAccountService
    {

        private readonly IAccountRepository _accountRepository;

        private readonly ILogger<AccountService> _logger;

        private readonly IStringLocalizer<IdentitySharedResource> _localizer;

        public AccountService(
            IAccountRepository accountRepository,
            ILogger<AccountService> logger,
            IStringLocalizer<IdentitySharedResource> localizer)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<Account> GetById(string id)
        {
            var account = await _accountRepository.GetByIdAsync(Guid.Parse(id));
            if (account == null)
            {
                _logger.LogError(_localizer["account_not_found"].Value);
                throw new BaseException(BaseError.ACCOUNT_NOT_FOUND, StatusCodes.Status404NotFound);
            }
            return account;
        }
    }
}
