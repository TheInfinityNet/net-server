using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.Services.Identity.Application.Exceptions;
using InfinityNetServer.Services.Identity.Application.IServices;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Application.Services
{
    public class LocalProviderService : ILocalProviderService
    {

        private readonly ILocalProviderRepository _localProviderRepository;

        private readonly ILogger<LocalProviderService> _logger;

        private readonly IStringLocalizer<IdentitySharedResource> _localizer;

        public LocalProviderService(
            ILocalProviderRepository localProviderRepository,
            ILogger<LocalProviderService> logger,
            IStringLocalizer<IdentitySharedResource> localizer)
        {
            _localProviderRepository = localProviderRepository;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<LocalProvider> GetByEmail(string email)
            => await _localProviderRepository.GetByEmailAsync(email);
    }
}
